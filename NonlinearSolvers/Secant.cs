using Functions;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class Secant
{
    public static Result<BigFloat> EvalR(IFunction function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat F(BigFloat x) => function.Eval(x);

        // Ustawiamy cele dokładności (opcjonalne, ale zgodne ze stylem EvalR)
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        // Początkowe obliczenia: korekta przedziału [a, b]
        BigFloat h = new BigFloat(0.179372) * (b - a);
        a += h;
        b -= h;

        BigFloat fa = F(a);
        BigFloat fb = F(b);

        // Jeśli |fa| < |fb|, zamieniamy punkty tak, by |fa| >= |fb|
        if (BigFloat.Abs(fa) < BigFloat.Abs(fb))
        {
            (a, b) = (b, a);
            (fa, fb) = (fb, fa);
        }

        epsilon = BigFloat.Abs(epsilon);
        BigFloat xn = b;
        int iterations = 0;

        while (iterations < mit)
        {
            iterations++;

            BigFloat denom = fa - fb;
            if (denom.IsZero)
            {
                return new Result<BigFloat>(
                    EvalStatus.DIVISION_BY_ZERO,
                    iterations,
                    BigFloat.Zero);
            }

            // wzór interpolacji liniowej:
            // x = b + fb * (b - a) / (fa - fb)
            xn = b + fb * (b - a) / denom;
            BigFloat fxn = F(xn);

            if (BigFloat.Abs(fxn) <= epsilon)
            {
                return new Result<BigFloat>(
                    EvalStatus.FULL_SUCCESS,
                    iterations,
                    xn);
            }

            // przesuwamy przedziały:
            fa = fb;
            fb = fxn;
            a = b;
            b = xn;
        }

        // brak zbieżności w wyznaczonej liczbie iteracji
        return new Result<BigFloat>(
            EvalStatus.NO_CONVERGENCE,
            iterations,
            xn);
    }

    public static Result<Interval> EvalI(IFunction function, BigFloat start, BigFloat end, int mit, BigFloat epsilon)
    {
    // Konwerter na przedziały
    Interval a = new Interval(start);
    Interval b = new Interval(end);
    Interval F(Interval x) => function.Eval(x);

    // Ustawienia dokładności BigFloat
    BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
    BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

    // 1) Przesunięcie krańców przedziału zgodnie z Pascalowym oryginałem
    Interval h = new Interval(new BigFloat(0.179372)) * (b - a);
    a += h;
    b -= h;

    // 2) Obliczenie wartości funkcji na skorygowanym przedziale
    Interval fa = F(a);
    Interval fb = F(b);

    // 3) Jeżeli funkcja nie zmienia znaku na [a,b], nie ma pierwiastka
    if (!(fa * fb).ContainsNegative())
    {
        return new Result<Interval>(EvalStatus.NO_SIGN_CHANGE, 0, null);
    }

    // 4) Jeżeli przekazanie dokładności, uproszczony warunek stopu
    epsilon = BigFloat.Abs(epsilon);
    int iterations = 0;

    // 5) Zamiana a<->b, fa<->fb, gdy |mid(fa)| < |mid(fb)| 
    //    (przybliżenie zgodne z porównaniem modułów w wersji nie-intervalnej)
    BigFloat midFa = BigFloat.Abs(fa.Midpoint());
    BigFloat midFb = BigFloat.Abs(fb.Midpoint());
    if (midFa < midFb)
    {
        (a, b) = (b, a);
        (fa, fb) = (fb, fa);
    }

    // 6) Iteracyjna interpolacja liniowa w pętli
    while (iterations < mit)
    {
        iterations++;

        Interval denom = fa - fb;
        if (denom.Contains(new BigFloat(0)))
        {
            return new Result<Interval>(EvalStatus.DIVISION_BY_ZERO, iterations, null);
        }

        // x = b + fb*(b-a)/(fa-fb)
        Interval xn = b + fb * (b - a) / denom;
        Interval fxn = F(xn);

        // Warunek stopu: szerokość przedziału wartości <= epsilon
        if (fxn.Width() <= epsilon)
        {
            return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, xn);
        }

        // Przesuwamy punkty: a←b, fa←fb; b←xn, fb←fxn
        a = b;    fa = fb;
        b = xn;   fb = fxn;
    }

    // Jeżeli nie udało się zbiec w limicie iteracji
    return new Result<Interval>(EvalStatus.NO_CONVERGENCE, iterations, b);
}
}