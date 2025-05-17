using System;
using Functions;
using Intervals;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public static class Bisection
{
    public static Result<BigFloat> Eval(IFunction function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        if ((F(a) * F(b)) > new BigFloat(0))
            throw new ArgumentException("Function doesnt change its sign between a and b");


        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);
        
        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        while (BigFloat.Abs(a - b) > epsilon)
        {
            iterations++;

            BigFloat mid = (a + b) / 2;
            if (BigFloat.Abs(F(mid)) < epsilon)
                return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, mid);

            if ((F(a) * F(mid)).Sign < 0)
            {
                b = mid;
            }
            else
            {
                a = mid;
            }

            if (iterations >= mit)
            {
                return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, mid);
            }

        }

        return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, (a + b) / 2);

        BigFloat F(BigFloat n) => function.Eval(n);
    }

    public static Result<Interval> Eval(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);

        if (!(F(a) * F(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        while (BigFloat.Abs(b.End - a.Start) > epsilon)
        {
            iterations++;

            BigFloat midPoint = (a.Start + b.End) / 2;
            Interval mid = new Interval(midPoint, midPoint);

            if (F(mid).Contains(0) && BigFloat.Abs(mid.End - mid.Start) < epsilon)
                return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, mid);

            if ((F(a) * F(mid)).ContainsNegative())
            {
                b = mid;
            }
            else
            {
                a = mid;
            }

            if (iterations >= mit)
            {
                return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, mid);
            }

        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, iterations, (a + b) / new Interval(2));

        Interval F(Interval n) => function.Eval(n);
    }



}
