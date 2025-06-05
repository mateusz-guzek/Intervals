using Functions;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

public class Secant
{
    public static Result<BigFloat> EvalR(IFunction function, BigFloat a, BigFloat b, int mit, BigFloat epsilon)
    {
        BigFloat F(BigFloat x) => function.Eval(x);
        
        
        

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
                    EvalStatus.ERROR,
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
            
            a = b;
            b = xn;
        }
        
        return new Result<BigFloat>(
            EvalStatus.NOT_ENOUGH_ITERATIONS,
            iterations,
            xn);
    }

    public static Result<Interval> EvalI(IFunction function, Interval a, Interval b, int mit, BigFloat epsilon)
    {
        Interval F(Interval x) => function.Eval(x);
        
        int i = 0;
        Interval c = new Interval(0);
        for (; i < mit; i++)
        {
            try
            {
                c = b - F(b) * (b - a) / (F(b) - F(a));
                (a, b) = (b, c);

                if (BigFloat.Abs(F(c).End) <= epsilon && c.Width() < epsilon)
                    break;
            }
            catch { break; }
        }

        if (i == 0)
        {
            return new Result<Interval>(EvalStatus.ERROR, i, null);
        }
        return new Result<Interval>(EvalStatus.FULL_SUCCESS, i, c);

    }
}