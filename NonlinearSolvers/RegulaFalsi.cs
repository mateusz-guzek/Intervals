using System;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class RegulaFalsi
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

        BigFloat x1 = (a * f(b) - b * f(a)) / (f(b) - f(a));


        while (iterations < mit)
        {
            iterations++;
            if ((f(a) * f(x1)).Sign < 0)
            {
                x1 = (x1 * f(a) - a * f(x1)) / (f(a) - f(x1));
            }
            else if ((f(b) * f(x1)).Sign < 0)
            {
                x1 = (x1 * f(b) - b * f(x1)) / (f(b) - f(x1));
            }
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

        Interval x1 = (a * f(b) - b * f(a)) / (f(b) - f(a));

        while (BigFloat.Abs(x1.Start - x1.End) > epsilon)
        {
            iterations++;
            if (!(f(a) * f(x1)).ContainsNegative())
            {
                x1 = (x1 * f(a) - a * f(x1)) / (f(a) - f(x1));
            }
            else if (!(f(b) * f(x1)).ContainsNegative())
            {
                x1 = (x1 * f(b) - b * f(x1)) / (f(b) - f(x1));
            }
            
            if(iterations == mit) break;
        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }
}