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
    private readonly IFunction first;
    private readonly IFunction second;
    private readonly IFunction third;

    public SimpleTests(ITestOutputHelper output)
    {
        this.first = new MyFunctions().Functions().ElementAt(0);
        this.second = new MyFunctions().Functions().ElementAt(1);
        this.third = new MyFunctions().Functions().ElementAt(2);
        this.output = output;
    }

    [Fact]
    public void FirstTestAuto()
    {
        BigFloat creationEpsilon = BigFloat.Parse("1e-128");
        BigFloat one = BigFloat.Parse("1.0");
        BigFloat two = BigFloat.Parse("2.0");
        Interval start = new Interval(one, one + creationEpsilon);
        Interval end = new Interval(two- creationEpsilon, two);
        BigFloat expected = BigFloat.Sqrt(2, AccuracyGoal.Absolute(16));
        
        var bisectionResult = Bisection.EvalI(first, start, end, 60, 1e-16);
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(first, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(first, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }
    [Fact]
    public void SecondTestAuto()
    {
        BigFloat creationEpsilon = BigFloat.Parse("1e-128");
        BigFloat one = BigFloat.Parse("-1.0");
        BigFloat two = BigFloat.Parse("1.0");
        Interval start = new Interval(one, one + creationEpsilon);
        Interval end = new Interval(two- creationEpsilon, two);
        BigFloat expected = BigFloat.Parse("0.31734758214650832");
        
        var bisectionResult = Bisection.EvalI(second, start, end, 60, 1e-16);
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(second, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(second, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }
    
    [Fact]
    public void ThirdTestAuto()
    {
        BigFloat creationEpsilon = BigFloat.Parse("1e-128");
        BigFloat one = BigFloat.Parse("0.1");
        BigFloat two = BigFloat.Parse("1.0");
        Interval start = new Interval(one, one + creationEpsilon);
        Interval end = new Interval(two- creationEpsilon, two);
        BigFloat expected = (BigFloat.GetPi(AccuracyGoal.Absolute(32))/new BigFloat(6)).RestrictPrecision(AccuracyGoal.Absolute(16), RoundingMode.TowardsNearest);
        
        var bisectionResult = Bisection.EvalI(third, start, end, 60, 1e-16);
        
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(third, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(third, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }
    
        [Fact]
    public void FirstTestManual()
    {
        BigFloat one = BigFloat.Parse("0.9999999");
        BigFloat one1 = BigFloat.Parse("1.0000001");
        BigFloat two = BigFloat.Parse("1.9999999");
        BigFloat two1 = BigFloat.Parse("2.0000001");
        Interval start = new Interval(one, one1);
        Interval end = new Interval(two, two1);
        BigFloat expected = BigFloat.Sqrt(2, AccuracyGoal.Absolute(16));
        
        var bisectionResult = Bisection.EvalI(first, start, end, 60, 1e-16);
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(first, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(first, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }
    [Fact]
    public void SecondTestManual()
    {
        BigFloat one = BigFloat.Parse("-1.0");
        BigFloat one1 = BigFloat.Parse("-0.9999999");
        BigFloat two = BigFloat.Parse("0.9999999");
        BigFloat two1 = BigFloat.Parse("1.00000001");
        Interval start = new Interval(one, one1);
        Interval end = new Interval(two, two1);
        BigFloat expected = BigFloat.Parse("0.31734758214650832");
        
        var bisectionResult = Bisection.EvalI(second, start, end, 60, 1e-16);
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(second, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(second, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }
    
    [Fact]
    public void ThirdTestManual()
    {
        BigFloat one = BigFloat.Parse("0.0999999");
        BigFloat one1 = BigFloat.Parse("0.1000001");
        BigFloat two = BigFloat.Parse("0.9999999");
        BigFloat two1 = BigFloat.Parse("1.0000001");
        Interval start = new Interval(one, one1);
        Interval end = new Interval(two, two1);
        BigFloat expected = (BigFloat.GetPi(AccuracyGoal.Absolute(32))/new BigFloat(6)).RestrictPrecision(AccuracyGoal.Absolute(16), RoundingMode.TowardsNearest);
        
        var bisectionResult = Bisection.EvalI(third, start, end, 60, 1e-16);
        
        Assert.True(bisectionResult.Value.Contains(expected));
        var regulaFalsiResult = RegulaFalsi.EvalI(third, start, end, 60, 1e-16);
        Assert.True(regulaFalsiResult.Value.Contains(expected));
        var secantResult = Secant.EvalI(third, start, end, 60, 1e-16);
        Assert.True(secantResult.Value.Contains(expected));
    }

    [Fact]
    public void RegulaFalsiError()
    {
        BigFloat one = BigFloat.Parse("2.9999999");
        BigFloat one1 = BigFloat.Parse("3.0000001");
        BigFloat two = BigFloat.Parse("3.9999999");
        BigFloat two1 = BigFloat.Parse("4.0000001");
        Interval start = new Interval(one, one1);
        Interval end = new Interval(two, two1);
        var regulaFalsiResult = RegulaFalsi.EvalI(second,start,end,60, 1e-16);
        Assert.True(regulaFalsiResult.Status == EvalStatus.NO_SIGN_CHANGE);
    }
    
    
    
    
}