using System;
using Functions;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class RegulaFalsi
{
    public static Result<BigFloat> EvalR(IFunction function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat F(BigFloat n) => function.Eval(n);

        if ((F(a) * F(b)) > new BigFloat(0))
            return new Result<BigFloat>(EvalStatus.NO_SIGN_CHANGE, 0, null);


        epsilon = BigFloat.Abs(epsilon);

        int iterations = 0;

        BigFloat x1 = (a * F(b) - b * F(a)) / (F(b) - F(a));


        while (BigFloat.Abs(F(x1)) > epsilon)
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

            if ((F(a) * F(x1)).Sign < 0)
            {
                b = x1;
            }
            else
            {
                a = x1;
            }

            if (iterations == mit)
                break;
        }

        return new Result<BigFloat>(EvalStatus.FULL_SUCCESS, iterations, x1);
    }

    public static Result<Interval> EvalI(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        Interval F(Interval n) => function.Eval(n);
        if (!(F(a) * F(b)).ContainsNegative())
            return new Result<Interval>(EvalStatus.NO_SIGN_CHANGE, 0, null);
        epsilon = BigFloat.Abs(epsilon);
        Interval c = a;
        int i = 0;
        for (; i < mit; i++)
        {
            Interval denom = F(b) - F(a);
            if(denom.Contains(0))
                return new Result<Interval>(EvalStatus.FULL_SUCCESS, i, c);
            
            c = b - (F(b) * (b-a)) / denom;

            if ((BigFloat.Abs(F(c).End)) < epsilon && c.Width() < epsilon)
                break;
            if ((F(a) * F(c)).ContainsNegative())
                b = c;
            else
                a = c;

        }

        return new Result<Interval>(EvalStatus.FULL_SUCCESS, i, c);
    }
}