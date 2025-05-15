using Nonlinear_Solvers;
using Numerics.NET;
using Nonlinear_Solvers;
using Interval = Intervals.Interval;
using Xunit;

namespace SolversTests;

public class RegulaFalsiTests
{
    [Fact]
    public void BigFloat_SolvesSimpleLinearEquation()
    {
        Func<BigFloat, BigFloat> f = x => x - 3;
        var result = RegulaFalsi.Eval(f, new BigFloat(1), new BigFloat(5), 100, new BigFloat(1e-20));

        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Assert.True(BigFloat.Abs(result.Value - 3) < new BigFloat(1e-15));
    }

    [Fact]
    public void BigFloat_ThrowsOnNoSignChange()
    {
        Func<BigFloat, BigFloat> f = x => x * x + 1; // No sign change in R

        Assert.Throws<ArgumentException>(() =>
        {
            RegulaFalsi.Eval(f, new BigFloat(-1), new BigFloat(1), 100, new BigFloat(1e-20));
        });
    }

    [Fact]
    public void Interval_SolvesSimpleLinearEquation()
    {
        Func<Interval, Interval> f = x => x - new Interval(3);
        var a = new Interval(1);
        var b = new Interval(5);
        var result = RegulaFalsi.Eval(f, a, b, 100, new BigFloat(1e-20));

        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Assert.True(result.Value.Contains(3));
    }

    [Fact]
    public void Interval_ThrowsOnNoSignChange()
    {
        Func<Interval, Interval> f = x => x.Sqr() + new Interval(1);
        var a = new Interval(-1);
        var b = new Interval(1);

        Assert.Throws<ArgumentException>(() =>
        {
            RegulaFalsi.Eval(f, a, b, 100, new BigFloat(1e-20));
        });
    }

    [Fact]
    public void BigFloat_QuadraticRoot()
    {
        Func<BigFloat, BigFloat> f = x => x * x - 2;
        var result = RegulaFalsi.Eval(f, new BigFloat(1), new BigFloat(2), 100, new BigFloat(1e-20));

        var sqrt2 = BigFloat.Sqrt(2);
        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Assert.True(BigFloat.Abs(result.Value - sqrt2) < new BigFloat(1e-15));
    }
}