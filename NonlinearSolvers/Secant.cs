using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class Secant
{
    public static Result<BigFloat> Eval(Func<BigFloat, BigFloat> f, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if ((f(a) * f(b)).Sign == 1)
        {
            throw new ArgumentException("Function doesnt change its sign between a and b");
        }


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x0 = a;
        BigFloat x1 = b;

        BigFloat xn = (f(x1) * x0 - f(x0) * x1) / (f(x1) - f(x0));
        x0 = x1;
        x1 = xn;

        while (iterations < mit && BigFloat.Abs(f(x1)) > epsilon)
        {
            iterations++;
            xn = (f(x1) * x0 - f(x0) * x1) / (f(x1) - f(x0));
            x0 = x1;
            x1 = xn;
        }

        return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }

    public static Result<Interval> Eval(Func<Interval, Interval> f, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(f(a) * f(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        Interval x0 = a;
        Interval x1 = b;

        Interval xn = (f(x1) * x0 - f(x0) * x1) / (f(x1) - f(x0));
        x0 = x1;
        x1 = xn;

        while (iterations < mit)
        {
            iterations++;
            xn = (f(x1) * x0 - f(x0) * x1) / (f(x1) - f(x0));
            x0 = x1;
            x1 = xn;
        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }
}