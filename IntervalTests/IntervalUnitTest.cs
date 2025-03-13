using Intervals;

namespace IntervalTests
{
    public class IntervalUnitTest
    {
        [Fact]
        public void Printing()
        {
            var interval = new Interval(1, 5);
            Assert.Equal("1...5", interval.ToString());

        }
        [Fact]
        public void Addition_Works_Correctly()
        {
            var A = new Interval(1.0, 3.0);
            var B = new Interval(2.0, 4.0);
            var result = A + B;
            Assert.Equal("3...7", result.ToString());
        }
        [Fact]
        public void Subtraction_Works_Correctly()
        {
            var A = new Interval(1.0, 3.0);
            var B = new Interval(2.0, 4.0);
            var result = A - B;
            Assert.Equal("-1...-1", result.ToString());
        }

        [Fact]
        public void Multiplication_Works_Correctly()
        {
            var A = new Interval(-2.0, 3.0);
            var B = new Interval(4.0, 5.0);
            var result = A * B;
            Assert.Equal("-8...15", result.ToString());
        }

        [Fact]
        public void Division_By_Zero_ThrowsException()
        {
            var A = new Interval(1.0, 3.0);
            var B = new Interval(-2.0, 2.0); // Contains zero

            Assert.Throws<Exception>(() => A / B);
        }
    }
}