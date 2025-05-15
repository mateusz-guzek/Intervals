using System;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class RegulaFalsi
{
    public static BigFloat Eval(Func<BigFloat, BigFloat> f, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (f(a) * f(b) > 0)
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x1 = (a * f(b) - b * f(a)) / (f(b) - f(a));

        while (BigFloat.Abs(f(x1)) > epsilon)
        {
            iterations++;
            if (f(a) * f(x1) < 0)
            {
                x1 = (x1 * f(a) - a * f(x1)) / (f(a) - f(x1));
            }
            else if (f(b) * f(x1) < 0)
            {
                x1 = (x1 * f(b) - b * f(x1)) / (f(b) - f(x1));
            }
        }

        return x1;
    }

    public static Interval Eval(Func<Interval, Interval> f, Interval a, Interval b, int mit, double epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(f(a) * f(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = Math.Abs(epsilon);

        int iterations = 0;

        while (BigFloat.Abs(b.End - a.Start) > epsilon)
        {
            iterations++;

            BigFloat midPoint = (a.Start + b.End) / 2;
            Interval mid = new Interval(midPoint, midPoint);

            if (f(mid).Contains(0) && BigFloat.Abs(mid.End - mid.Start) < epsilon)
                return mid;

            if ((f(a) * f(mid)).ContainsNegative())
            {
                b = mid;
            }
            else
            {
                a = mid;
            }

            if (iterations >= mit)
            {
                return mid;
            }
        }

        return (a + b) / new Interval(2);
    }
}