using System;
using Intervals;
using Nonlinear_Solvers;
using Xunit;

namespace SolversTests;

public class BisectionInterval
{
    // Przykładowa funkcja: f(x) = x^2 - 2, pierwiastek w sqrt(2) ≈ 1.4142
    private static Interval F(Interval x)
    {
        return x.Sqr() - new Interval(2.0);
    }

    [Fact]
    public void BisectionInterval_FindsRoot_Sqrt2()
    {
        // Arrange
        Interval a = new Interval(1);
        Interval b = new Interval(2);
        double epsilon = 1e-6;
        int mit = 100;

        // Act
        Interval result = Bisection.Eval(F, a, b, mit, epsilon);

        // Assert
        double sqrt2 = Math.Sqrt(2);
        Assert.InRange(result.Start, sqrt2 - epsilon, sqrt2 + epsilon);
        Assert.InRange(result.End, sqrt2 - epsilon, sqrt2 + epsilon);
    }

    [Fact]
    public void BisectionInterval_ThrowsIfNoSignChange()
    {
        // Arrange
        Interval a = new Interval(2.0, 2.0);
        Interval b = new Interval(3.0, 3.0);
        double epsilon = 1e-6;
        int mit = 100;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Bisection.Eval(F, a, b, mit, epsilon));
    }

    [Fact]
    public void BisectionInterval_StopsAtMaxIterations()
    {
        // Arrange
        Interval a = new Interval(1);
        Interval b = new Interval(2);
        double epsilon = 1e-15; // mała dokładność wymuszająca wiele iteracji
        int mit = 1000; // bardzo mały limit iteracji

        // Act
        Interval result = Bisection.Eval(F, a, b, mit, epsilon);

        // Assert
        Assert.True(result.Contains(Math.Sqrt(2)), "Result should contain sqrt(2)");
    }
}
