using Functions;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace UserFunctions;

public class MyFunctions : IMyFunctions
{
    public IEnumerable<IFunction> Functions()
    {
        return new List<IFunction>()
        {
            new UserFunction("f(x) = x^2 - 2", 
                x => BigFloat.Pow(x,2) - new BigFloat(2),
                x => x.Sqr() - new Interval(2)
                ),
            new UserFunction("f(x) = x * e^(sqrt(x+1))-1", 
                x => x * BigFloat.Exp(BigFloat.Sqrt(x+1)) - new BigFloat(1),
                x => x * x.Exp() * new Interval(1).Exp()),
            new UserFunction("f(x) = cos(x)",
                x =>
                {
                    BigFloat s = BigFloat.Sin(x);
                    return s*(s+new BigFloat(0.5)) - new BigFloat(0.5);
                },
                x =>
                {
                    Interval s = x.Sin();
                    return s*(s + new Interval(0.5)) - new Interval(0.5);
                }
            )
        };
    }
}