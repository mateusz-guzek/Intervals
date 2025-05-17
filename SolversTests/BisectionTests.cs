using System;
using Xunit;
using Nonlinear_Solvers;
using Numerics.NET;
using Xunit.Abstractions;
using Interval = Intervals.Interval;

namespace SolversTests
{
    public class BisectionTests
    {
        // [Fact]
        // public void Bisection_Interval_SimplePolynomial_ShouldFindRoot()
        // {
        //     // f(x) = x^2 - 2
        //     Func<Interval, Interval> f = x => x * x - new Interval(2.0);
        //
        //     Interval a = new Interval(1.0, 1.0);
        //     Interval b = new Interval(2.0, 2.0);
        //     int maxIter = 100;
        //     double epsilon = 1e-20;
        //
        //     var result = Bisection.Eval(f, a, b, maxIter, epsilon);
        //
        //     Console.WriteLine(result.Iterations);
        //     Console.WriteLine(result.Value.Start);
        //     Console.WriteLine(BigFloat.Sqrt(2));
        //     Console.WriteLine(Math.Sqrt(2));
        //     Console.WriteLine(result.Value.End);
        //     Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
        //     Assert.True(result.Value.Contains(BigFloat.Sqrt(2)));
        //     Assert.True(result.Value.Width() < 2 * epsilon);
        // }

        [Fact]
        public void Bisection_Interval_ShouldThrow_WhenNoSignChange()
        {
            // f(x) = x^2 + 1 has no real roots
            Func<Interval, Interval> f = x => x.Sqr() + new Interval(1.0);

            Interval a = new Interval(-1);
            Interval b = new Interval(1);
            int maxIter = 100;
            double epsilon = 1e-10;

            Assert.Throws<ArgumentException>(() =>
                Bisection.Eval(f, a, b, maxIter, epsilon));
        }

        [Fact]
        public void Bisection_Interval_SinFunction_ShouldFindRootNearZero()
        {
            // f(x) = sin(x)
            Func<Interval, Interval> f = x => x.Sin();

            Interval a = new Interval(-1.0, -1.0);
            Interval b = new Interval(1.0, 1.0);
            int maxIter = 100;
            BigFloat epsilon = 1e-10;

            var result = Bisection.Eval(f, a, b, maxIter, epsilon);
            
            Console.WriteLine(result);

            Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
            Assert.True(result.Value.Contains(0));
            Assert.True(result.Value.Width() < 2 * epsilon);
        }

        [Fact]
        public void Bisection_Interval_ConvergesBeforeMaxIterations()
        {
            // f(x) = x - 0.001, root at 0.001
            Func<Interval, Interval> f = x => x - new Interval(0.001);

            Interval a = new Interval(0.0, 0.0);
            Interval b = new Interval(1.0, 1.0);
            int maxIter = 1000;
            double epsilon = 1e-20;

            var result = Bisection.Eval(f, a, b, maxIter, epsilon);
            
            Console.WriteLine(result.Value.Start);
            Console.WriteLine(result.Value.End);

            Assert.Equal(EvalStatus.FULL_SUCCESS, result.Status);
            Assert.True(result.Value.Contains(0.001));
            Assert.True(result.Iterations < maxIter);
        }
    }
}