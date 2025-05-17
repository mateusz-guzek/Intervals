using Nonlinear_Solvers;
using Numerics.NET;
using Xunit;
using Interval = Intervals.Interval;

namespace SolversTests;

public class SecantTests
{
    [Fact]
    public void BigFloat_SolvesLinearFunction()
    {
        Func<BigFloat, BigFloat> f = x => x - 5;
        var result = Secant.Eval(f, new BigFloat(4), new BigFloat(6), 100, new BigFloat(1e-20));

        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Assert.True(BigFloat.Abs(result.Value - 5) < new BigFloat(1e-15));
    }

    [Fact]
    public void BigFloat_ThrowsOnNoSignChange()
    {
        Func<BigFloat, BigFloat> f = x => x * x + 1; // zawsze dodatnia
        Assert.Throws<ArgumentException>(() =>
        {
            Secant.Eval(f, new BigFloat(-1), new BigFloat(1), 100, new BigFloat(1e-20));
        });
    }

    [Fact]
    public void BigFloat_SolvesQuadraticRoot()
    {
        Func<BigFloat, BigFloat> f = x => x - 2;
        var result = Secant.Eval(f, new BigFloat(1), new BigFloat(2), 100, new BigFloat(1e-20));

        var sqrt2 = BigFloat.Sqrt(2);
        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Console.WriteLine(result.Value);
        Console.WriteLine(sqrt2);
        Assert.True(BigFloat.Abs(result.Value - sqrt2) < new BigFloat(1e-15));
    }

    [Fact]
    public void Interval_ThrowsOnNoSignChange()
    {
        Func<Interval, Interval> f = x => x.Sqr() + new Interval(1); // zawsze dodatnia
        Assert.Throws<ArgumentException>(() =>
        {
            Secant.Eval(f, new Interval(-2), new Interval(2), 100, new BigFloat(1e-20));
        });
    }

    [Fact]
    public void Interval_SolvesQuadraticRoot()
    {
        Func<Interval, Interval> f = x => x.Sqr() - new Interval(2);
        var result = Secant.Eval(f, new Interval(1), new Interval(2), 100, new BigFloat(1e-20));

        var expected = BigFloat.Sqrt(2);
        Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        Assert.True(result.Value.Contains(expected));
    }
}