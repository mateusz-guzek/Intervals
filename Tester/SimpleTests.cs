using Functions;
using Nonlinear_Solvers;
using Numerics.NET;
using UserFunctions;
using Xunit.Abstractions;
using Interval = Intervals.Interval;

namespace Tester;

public class SimpleTests
{
    
    private readonly ITestOutputHelper output;

    public SimpleTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void BisectionInterval()
    {
        MyFunctions f = new MyFunctions();
        Result<Interval> result1 = Bisection.EvalI(f.Functions().ElementAt(0), new Interval(1.0), new Interval(2.0), 100, 1e-16);
        Result<Interval> result2 = Bisection.EvalI(f.Functions().ElementAt(1), new Interval(-0.9), new Interval(2.0), 100, 1e-16);
        Result<Interval> result3 = Bisection.EvalI(f.Functions().ElementAt(2), new Interval(0.5), new Interval(1.0), 100, 1e-16);
        Assert.True(result2.Value.Contains(0.3173475821465082));
        Assert.True(result3.Value.Contains(new BigFloat(Math.PI)/6));
        Assert.True(result1.Value.Contains(BigFloat.Sqrt(2)));
        
    }
    
    [Fact]
    public void FalsiInterval()
    {
        MyFunctions f = new MyFunctions();
        Result<Interval> result1 = RegulaFalsi.EvalI(f.Functions().ElementAt(0), new Interval(1.0), new Interval(2.0), 60, 1e-16);
        Result<Interval> result2 = RegulaFalsi.EvalI(f.Functions().ElementAt(1), new Interval(-0.9), new Interval(1.0), 100, 1e-16);
        Result<Interval> result3 = RegulaFalsi.EvalI(f.Functions().ElementAt(2), new Interval(0.5), new Interval(1.0), 100, 1e-16);
        Assert.True(result1.Value.Contains(1.41421356237310));
        Assert.True(result2.Value.Contains(0.317347582146508));
        Assert.True(result3.Value.Contains(0.523598775598299));
    }
    
    [Fact]
    public void SecantInterval()
    {
        MyFunctions f = new MyFunctions();
        Result<Interval> result1 = Secant.EvalI(f.Functions().ElementAt(0), new Interval(1.0), new Interval(2.0), 60, 1e-16);
        Result<Interval> result2 = Secant.EvalI(f.Functions().ElementAt(1), new Interval(3.0), new Interval(4.0), 100, 1e-16);
        Result<Interval> result3 = Secant.EvalI(f.Functions().ElementAt(2), new Interval(0.5), new Interval(1.0), 100, 1e-16);
        Assert.True(result1.Value.Contains(1.41421356237310));
        Assert.True(result2.Value.Contains(0.317347582146508));
        Assert.True(result3.Value.Contains(0.523598775598299));
    }
    
}