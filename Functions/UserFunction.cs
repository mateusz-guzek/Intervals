using Numerics.NET;
using Interval = Intervals.Interval;

namespace Functions;

public class UserFunction : IFunction
{
    
    private readonly Func<BigFloat, BigFloat> evalReal;
    private readonly Func<Interval, Interval> evalInterval;
    private readonly string repr;

    public UserFunction(string representation, Func<BigFloat, BigFloat> f1, Func<Interval, Interval> f2)
    {
        repr = representation;
        evalReal = f1;
        evalInterval = f2;
    }
    
    public BigFloat Eval(BigFloat x) => evalReal(x);

    public Interval Eval(Interval x) => evalInterval(x);

    public string StringRepresentation => repr;
}