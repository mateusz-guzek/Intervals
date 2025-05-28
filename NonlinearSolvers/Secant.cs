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
        

        // Jeśli |fa| < |fb|, zamieniamy punkty tak, by |fa| >= |fb|
        if (BigFloat.Abs(F(a)) < BigFloat.Abs(F(b)))
        {
            (a, b) = (b, a);
        }

        epsilon = BigFloat.Abs(epsilon);
        BigFloat xn = b;
        int iterations = 0;

        while (iterations < mit)
        {
            iterations++;

            BigFloat denom = F(a) - F(b);
            if (denom.IsZero)
            {
                return new Result<BigFloat>(
                    EvalStatus.DIVISION_BY_ZERO,
                    iterations,
                    BigFloat.Zero);
            }

            // wzór interpolacji liniowej:
            // x = b + fb * (b - a) / (fa - fb)
            xn = b + F(b) * (b - a) / denom;
            BigFloat fxn = F(xn);

            if (BigFloat.Abs(fxn) <= epsilon)
            {
                return new Result<BigFloat>(
                    EvalStatus.FULL_SUCCESS,
                    iterations,
                    xn);
            }

            // przesuwamy przedziały:
            a = b;
            b = xn;
        }

        // brak zbieżności w wyznaczonej liczbie iteracji
        return new Result<BigFloat>(
            EvalStatus.NO_CONVERGENCE,
            iterations,
            xn);
    }

    public static Result<Interval> EvalI(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        Interval F(Interval x) => function.Eval(x);
        
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        List<Interval> intervals = new List<Interval>();

        

        
        // if (!(F(a) * F(b)).ContainsNegative())
        // {
        //     return new Result<Interval>(EvalStatus.NO_SIGN_CHANGE, 0, null);
        // }

        Interval outcache = new Interval(0);
        int iterations = 0;
        do
        {
            iterations++;
            Interval denom = F(a) - F(b);
            if (denom.Contains(0))
            {
                return new Result<Interval>(EvalStatus.DIVISION_BY_ZERO, iterations, intervals.MinBy(interval => interval.Width()));
            }
            Interval xn = (F(a) * b - F(b) * a) / denom;
            if(F(xn).Contains(0))
                intervals.Add(xn);
            outcache = xn;

            if (F(xn).Width() < epsilon && F(xn).Contains(0))
            {
                return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, xn);
            }

            a = b;
            b = xn;

        } while(iterations < mit);
        
        return new Result<Interval>(EvalStatus.NOT_ACCURATE, iterations, outcache);
    }
}