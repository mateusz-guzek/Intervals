using Functions;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class Secant
{
    public static Result<BigFloat> Eval(IFunction function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat F(BigFloat n) => function.Eval(n);

        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        BigFloat fa = F(a); // -1
        BigFloat fb = F(b); // 13

        if ((fa * fb) > BigFloat.Zero)
        {
            return new Result<BigFloat>(EvalStatus.NO_SIGN_CHANGE, 0, BigFloat.Zero);
        }

        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x0 = a; // 1
        BigFloat x1 = b; // 4
        BigFloat xn = x1; // 4

        while (iterations < mit)
        {
            iterations++;
            BigFloat fx0 = F(x0);
            BigFloat fx1 = F(x1);

            BigFloat denominator = fx1 - fx0;

            if (denominator.IsZero)
            {
                return new Result<BigFloat>(EvalStatus.DIVISION_BY_ZERO, iterations, BigFloat.Zero);
            }

            xn = (fx1 * x0 - fx0 * x1) / denominator;

            // Warunek zbieżności
            if (BigFloat.Abs(F(xn)) <= epsilon)
            {
                Console.WriteLine(xn.GetDecimalDigits());
                return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, xn);
            }

            x0 = x1;
            x1 = xn;
            
        }

        return new Result<BigFloat>(EvalStatus.NO_CONVERGENCE, iterations, xn);
    }


    public static Result<Interval> Eval(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        Interval F(Interval n) => function.Eval(n);

        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(F(a) * F(b)).ContainsNegative())
            return new Result<Interval>(EvalStatus.NO_SIGN_CHANGE, 0, null);


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        Interval x0 = a;
        Interval x1 = b;

        Interval xn = (F(x1) * x0 - F(x0) * x1) / (F(x1) - F(x0));
        x0 = x1;
        x1 = xn;

        while (iterations < mit)
        {
            iterations++;
            xn = (F(x1) * x0 - F(x0) * x1) / (F(x1) - F(x0));
            x0 = x1;
            x1 = xn;

            if (x1.Width() < epsilon)
            {
                return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);
            }
        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }
}