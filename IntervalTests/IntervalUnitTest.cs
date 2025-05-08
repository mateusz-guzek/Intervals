using Interval = Intervals.Interval;

namespace IntervalTests
{
    public class IntervalUnitTest
    {
        [Fact]
        public void Constructor_SetsStartAndEnd()
        {
            var interval = new Interval(1.0, 2.0);
            Assert.Equal(1.0, interval.Start);
            Assert.Equal(2.0, interval.End);
        }

        [Fact]
        public void Constructor_ThrowsIfStartGreaterThanEnd()
        {
            Assert.Throws<ArgumentException>(() => new Interval(2.0, 1.0));
        }

        [Fact]
        public void Contains_ChecksCorrectly()
        {
            var interval = new Interval(-1, 2);
            Assert.True(interval.Contains(0));
            Assert.False(interval.Contains(-2));
        }

        [Fact]
        public void Width_ComputesCorrectly()
        {
            var interval = new Interval(3.5, 5.5);
            Assert.Equal(2.0, interval.Width());
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 4, 6)]
        public void Addition_Works(double a1, double b1, double a2, double b2, double exStart, double exEnd)
        {
            var result = new Interval(a1, b1) + new Interval(a2, b2);
            Assert.Equal(exStart, ((double)result.Start), 5);
            Assert.Equal(exEnd, ((double)result.End), 5);
        }

        [Fact]
        public void Multiplication_Works()
        {
            var interval1 = new Interval(-2, 3);
            var interval2 = new Interval(4, 5);
            var result = interval1 * interval2;
            Assert.Equal(-10, ((double)result.Start), 5);
            Assert.Equal(15, ((double)result.End), 5);
        }

        [Fact]
        public void Division_Works()
        {
            var a = new Interval(2, 4);
            var b = new Interval(1, 2);
            var result = a / b;
            Assert.Equal(1, ((double)result.Start), 5);
            Assert.Equal(4, ((double)result.End), 5);
        }

        [Fact]
        public void Division_ByIntervalContainingZero_Throws()
        {
            var a = new Interval(1, 2);
            var b = new Interval(-1, 1);
            Assert.Throws<DivideByZeroException>(() => a / b);
        }

        [Fact]
        public void Sqr_CorrectForPositive()
        {
            var interval = new Interval(2, 3);
            var result = interval.Sqr();
            Assert.Equal(4, ((double)result.Start), 5);
            Assert.Equal(9, ((double)result.End), 5);
        }

        [Fact]
        public void Sqr_CorrectForNegative()
        {
            var interval = new Interval(-3, -2);
            var result = interval.Sqr();
            Assert.Equal(4, ((double)result.Start), 5);
            Assert.Equal(9, ((double)result.End), 5);
        }

        [Fact]
        public void Sqr_CorrectForMixed()
        {
            var interval = new Interval(-2, 3);
            var result = interval.Sqr();
            Assert.Equal(0, ((double)result.Start), 5);
            Assert.Equal(9, ((double)result.End), 5);
        }

        [Fact]
        public void Sqrt_Works()
        {
            var interval = new Interval(4, 9);
            var result = interval.Sqrt();
            Assert.Equal(2, ((double)result.Start), 5);
            Assert.Equal(3, ((double)result.End), 5);
        }

        [Fact]
        public void Sqrt_ThrowsOnNegative()
        {
            var interval = new Interval(-1, 4);
            Assert.Throws<ArgumentException>(() => interval.Sqrt());
        }

        // works 2 = 1.99999999999999992304520406883380267060366498657872021904290841481584591455724080845303138906228374110220725028743257519183117574466911285822063008319361192421171800059377842540254810220873259984245070
        // [Fact]
        // public void SqrtN_Works()
        // {
        //     var interval = new Interval(8, 27);
        //     var result = interval.SqrtN(3);
        //     Console.WriteLine(Numerics.NET.BigFloat.Pow(new Numerics.NET.BigFloat(8),1/3));
        //     Assert.InRange(result.Start, 2.0, 2.1);
        //     Assert.InRange(result.End, 3.0, 3.1);
        // }

        [Fact]
        public void ContainsPositiveNegative_Works()
        {
            var interval = new Interval(-2, 3);
            Assert.True(interval.ContainsPositive());
            Assert.True(interval.ContainsNegative());

            var pos = new Interval(1, 2);
            Assert.True(pos.ContainsPositive());
            Assert.False(pos.ContainsNegative());

            var neg = new Interval(-5, -1);
            Assert.False(neg.ContainsPositive());
            Assert.True(neg.ContainsNegative());
        }

        [Fact]
        public void Sin_WorksRoughlyCorrectly()
        {
            var interval = new Interval(0, Math.PI / 2);
            var result = interval.Sin();
            Console.WriteLine(result);
            Assert.True(result.Start >= 0);
            Assert.True(result.End <= 1);
        }

        [Fact]
        public void Cos_WorksRoughlyCorrectly()
        {
            var interval = new Interval(0, Math.PI / 2);
            var result = interval.Cos();
            Assert.True(result.Start >= 0);
            Assert.True(result.End <= 1);
        }

        [Fact]
        public void Exp_Works()
        {
            var interval = new Interval(0, 1);
            var result = interval.Exp();

            Assert.True(result.Contains(1));
            Assert.True(result.Contains(Math.E));
        }

        [Fact]
        public void Constants_AreReasonable()
        {
            Assert.InRange(Interval.Pi.Start, 3.1415, 3.1416);
            Assert.InRange(Interval.Sqrt2.Start * Interval.Sqrt2.Start, 1.999, 2.001);
        }
    }
}