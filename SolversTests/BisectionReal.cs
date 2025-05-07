using System;
using Xunit;
using Nonlinear_Solvers;

namespace SolversTests
{
    public class SolversUnitTest
    {
        // f(x) = x^2 - 2 ma miejsce zerowe w x = sqrt(2) ≈ 1.4142

    //     [Fact]
    //     public void Bisection_FindsRootOfXSquareMinus2()
    //     {
    //         // Arrange
    //         Func<double, double> f = x => x * x - 2;
    //         double a = 1.0;
    //         double b = 2.0;
    //         int mit = 60;
    //         double epsilon = 1e-10;

    //         // Act
    //         double result = Bisection.Eval(f, a, b, mit, epsilon);

    //         // Assert
    //         Assert.InRange(result, Math.Sqrt(2) - epsilon, Math.Sqrt(2) + epsilon);
    //     }

    //     [Fact]
    //     public void Bisection_ThrowsException_WhenNoSignChange()
    //     {
    //         // Arrange
    //         Func<double, double> f = x => x * x - 2;
    //         double a = 2.0;
    //         double b = 3.0;
    //         int mit = 60;
    //         double epsilon = 1e-6;

    //         // Act & Assert
    //         Assert.Throws<ArgumentException>(() => Bisection.Eval(f, a, b, mit, epsilon));
    //     }

    //     [Fact]
    //     public void Bisection_HandlesNegativeEpsilon()
    //     {
    //         // Arrange
    //         Func<double, double> f = x => x * x - 2;
    //         double a = 1.0;
    //         double b = 2.0;
    //         int mit = 60;
    //         double epsilon = -1e-6; // negative input

    //         // Act
    //         double result = Bisection.Eval(f, a, b, mit, epsilon);

    //         // Assert
    //         Assert.InRange(result, Math.Sqrt(2) - Math.Abs(epsilon), Math.Sqrt(2) + Math.Abs(epsilon));
    //     }
    }
}
