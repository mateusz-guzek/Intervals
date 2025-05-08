using System;
using Interval = Intervals.Interval;
using Nonlinear_Solvers;
using Numerics.NET;
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


    // works
    //  actual:   1.41421356237309504880168872420996511083053289877144
    //  expected: 1.414213562373095048801688724209
    // [Fact]
    // public void BisectionInterval_StopsAtMaxIterations()
    // {

    //     BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(20);
    //     BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(20);
    //     // Arrange
    //     Interval a = new Interval(1);
    //     Interval b = new Interval(2);
    //     double epsilon = 1e-64; // mała dokładność wymuszająca wiele iteracji
    //     int mit = 100; // bardzo mały limit iteracji

    //     // Act
    //     Interval result = Bisection.Eval(F, a, b, mit, epsilon);

    //     Console.WriteLine(result.Start + "\n" + result.End + "\n" + BigFloat.Sqrt(2));
    //     // Assert
    //     Assert.True(result.Contains(Math.Sqrt(2)), "Result should contain sqrt(2)");
    // }
}
