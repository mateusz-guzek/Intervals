namespace Intervals
{


    /// <summary>
    /// Class for interval arithmetics. See 
    /// https://en.wikipedia.org/wiki/Interval_arithmetic for reference
    /// </summary>
    public class Interval
    {
        private Double _start;
        private Double _end;


        public Interval(Double start, Double end)
        {
            _start = start;
            _end = end;
        }
        public override string ToString()
        {
            return $"{_start}...{_end}";
        }
        public static Interval operator +(Interval left, Interval right)
        {
            return new Interval(left._start + right._start, left._end + right._end);
        }
        public static Interval operator -(Interval left, Interval right)
        {
            return new Interval(left._start - right._start, left._end - right._end);
        }
        public static Interval operator *(Interval left, Interval right)
        {
            double minLeft = Math.Min(left._start, left._end);
            double minRight = Math.Min(right._start, right._end);

            double maxLeft = Math.Max(left._start, left._end);
            double maxRight = Math.Max(right._start, right._end);

            return new Interval(minLeft * minRight, maxLeft * maxRight);
        }

        public Interval Inverse()
        {
            if (_start <= 0 && _end >= 0)
            {
                throw new Exception("Cannot inverse interval which contains zero.");
            }


            return new Interval(1 / _start, 1 / _end);
        }

        public static Interval operator /(Interval left, Interval right)
        {
            return left * right.Inverse();
        }


    }
}
