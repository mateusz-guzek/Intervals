using System;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class RegulaFalsi
{
    public static Result<BigFloat> Eval(Function function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);
        
        if ((F(a) * F(b)).Sign == 1)
        {
            throw new ArgumentException("Function doesnt change its sign between a and b");
        }


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x1 = (a * F(b) - b * F(a)) / (F(b) - F(a));


        while (iterations < mit)
        {
            iterations++;
            if ((F(a) * F(x1)).Sign < 0)
            {
                x1 = (x1 * F(a) - a * F(x1)) / (F(a) - F(x1));
            }
            else if ((F(b) * F(x1)).Sign < 0)
            {
                x1 = (x1 * F(b) - b * F(x1)) / (F(b) - F(x1));
            }
        }

        return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, x1);

        BigFloat F(BigFloat n) => function.Eval(n);
    }

    public static Result<Interval> Eval(Function function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(F(a) * F(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        Interval x1 = (a * F(b) - b * F(a)) / (F(b) - F(a));

        while (BigFloat.Abs(x1.Start - x1.End) > epsilon)
        {
            iterations++;
            if (!(F(a) * F(x1)).ContainsNegative())
            {
                x1 = (x1 * F(a) - a * F(x1)) / (F(a) - F(x1));
            }
            else if (!(F(b) * F(x1)).ContainsNegative())
            {
                x1 = (x1 * F(b) - b * F(x1)) / (F(b) - F(x1));
            }
            
            if(iterations == mit) break;
        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);

        Interval F(Interval n) => function.Eval(n);
    }
}