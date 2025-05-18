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

        if ((F(a) * F(b)).Sign == 1)
        {
            throw new ArgumentException("Function doesnt change its sign between a and b");
        }


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x0 = a;
        BigFloat x1 = b;

        BigFloat xn = (F(x1) * x0 - F(x0) * x1) / (F(x1) - F(x0));
        x0 = x1;
        x1 = xn;

        while (iterations < mit && BigFloat.Abs(F(x1)) > epsilon)
        {
            iterations++;
            xn = (F(x1) * x0 - F(x0) * x1) / (F(x1) - F(x0));
            x0 = x1;
            x1 = xn;
        }

        return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }

    public static Result<Interval> Eval(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        Interval F(Interval n) => function.Eval(n);
        
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(F(a) * F(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


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
        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);
        
    }
}