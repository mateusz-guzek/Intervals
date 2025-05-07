using Intervals;
using PeterO.Numbers;

namespace IntervalTests
{
    public class IntervalUnitTest
    {
        [Fact]
        public void TestAddition()
        {
            var a = new Interval(1, 2);
            var b = new Interval(3, 4);
            var result = a + b;
            Assert.Equal(4, result.Start);
            Assert.Equal(6, result.End);
        }

        [Fact]
        public void TestSubtraction()
        {
            var a = new Interval(5, 7);
            var b = new Interval(2, 3);
            var result = a - b;
            Assert.Equal(2, result.Start);
            Assert.Equal(5, result.End);
        }

        [Fact]
        public void TestMultiplication()
        {
            var a = new Interval(1, 2);
            var b = new Interval(3, 4);
            var result = a * b;
            Assert.Equal(3, result.Start);
            Assert.Equal(8, result.End);
        }

        [Fact]
        public void TestDivision()
        {
            var a = new Interval(4, 6);
            var b = new Interval(2, 3);
            var result = a / b;
            Assert.Equal(4.0 / 3.0, result.Start, 10);
            Assert.Equal(6.0 / 2.0, result.End, 10);
        }

        [Fact]
        public void TestDivisionByZeroThrows()
        {
            var a = new Interval(1, 2);
            var b = new Interval(-1, 1);
            Assert.Throws<DivideByZeroException>(() => a / b);
        }

        [Fact]
        public void TestContains()
        {
            var interval = new Interval(1, 3);
            Assert.True(interval.Contains(2));
            Assert.True(interval.Contains(1));
            Assert.True(interval.Contains(3));
            Assert.False(interval.Contains(0.9999));
            Assert.False(interval.Contains(3.0001));
        }

        [Fact]
        public void TestWidth()
        {
            var interval = new Interval(5.5, 7.0);
            Assert.Equal(1.5, interval.Width(), 10);
        }

        [Fact]
        public void TestSqr()
        {
            var interval = new Interval(-2, 3);
            var result = interval.Sqr();
            Assert.Equal(0, result.Start, 10);
            Assert.Equal(9, result.End, 10);
        }

        [Fact]
        public void TestSqrt()
        {
            var interval = new Interval(4, 9);
            var result = interval.Sqrt();
            Assert.Equal(2, result.Start, 10);
            Assert.Equal(3, result.End, 10);
        }

        [Fact]
        public void TestSqrtNegativeThrows()
        {
            var interval = new Interval(-1, 4);
            Assert.Throws<ArgumentException>(() => interval.Sqrt());
        }

        [Fact]
        public void TestSqrtN()
        {
            var interval = new Interval(1, 27);
            var result = interval.SqrtN(3); // cube root
            Assert.Equal(1, result.Start, 10);
            Assert.Equal(3, result.End, 10);
        }

        [Fact]
        public void TestExp()
        {
            var interval = new Interval(0, 1);
            var result = interval.Exp();
            Assert.True(result.Start >= 1);
            Assert.True(result.End >= Math.E - 0.01 && result.End <= Math.E + 0.01);
        }

        [Fact]
        public void TestSinClamping()
        {
            var interval = new Interval(0, Math.PI);
            var result = interval.Sin();
            Assert.True(result.Start >= -1);
            Assert.True(result.End <= 1);
        }

        [Fact]
        public void TestCosClamping()
        {
            var interval = new Interval(0, Math.PI);
            var result = interval.Cos();
            
            Assert.True(result.Start >= -1);
            Assert.True(result.End <= 1);
        }
    }
}