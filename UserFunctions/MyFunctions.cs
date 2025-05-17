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
            new UserFunction("x^2", 
                x => BigFloat.Pow(x,2) - 1,
                x => x.Sqr() - new Interval(1)
                ),
            new UserFunction("sin(x)", 
                x => BigFloat.Sin(x),
                x => x.Sin()
            )
        };
    }
}