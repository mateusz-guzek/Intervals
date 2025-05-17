using Numerics.NET;
using Interval = Intervals.Interval;

namespace Functions;

public interface IFunction
{
    public BigFloat Eval(BigFloat x);
    
    public Interval Eval(Interval x);

    public String StringRepresentation
    {
        get;
    }
}
