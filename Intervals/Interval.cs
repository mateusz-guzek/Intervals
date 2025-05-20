using Numerics.NET;

namespace Intervals
{


    /// <summary>
    /// Class for interval arithmetics. See 
    /// https://en.wikipedia.org/wiki/Interval_arithmetic for reference
    /// </summary>
    /// 
    /// width   x
    /// add     x
    /// sub     x
    /// mul     x
    /// div     x
    /// 
    /// sin     x
    /// cos     x
    /// exp     x
    /// sqr     x
    /// sqrtN   x
    /// pi      x
    /// 
    /// 
    /// 
    public class Interval
    {

        private BigFloat epsilon = 1e-20;

        private BigFloat start;
        private BigFloat end;

        public BigFloat Start => start;
        public BigFloat End => end;



        public Interval(BigFloat start, BigFloat end)
        {
            if (start.CompareTo(end) > 0)
                throw new ArgumentException("Start cannot be greater than end.");

            this.start = start;
            this.end = end;
        }

        public Interval(string number)
        {
            BigFloat parsed = BigFloat.Parse(number);
            this.start = parsed - epsilon;
            this.end = parsed + epsilon;
        }

        public Interval(string start, string end)
        {
            BigFloat x = BigFloat.Parse(start);
            BigFloat y = BigFloat.Parse(end);
            
            this.start = x;
            this.end = y;
        }

        public Interval(BigFloat number)
        {
            start = number - epsilon;
            end = number + epsilon;
        }

        public bool Contains(BigFloat number)
        {
            return start <= number && end >= number;
        }

        public BigFloat Width()
        {
            return end - start;
        }

        public override string ToString()
        {
            return $"[{start} , {end}]";
        }

        public static Interval operator +(Interval left, Interval right)
        {
            return new Interval(left.start + right.start, left.end + right.end);
        }

        public static Interval operator -(Interval left, Interval right)
        {
            return new Interval(left.start - right.end, left.end - right.start);
        }

        public static Interval operator *(Interval left, Interval right)
        {
            BigFloat a = left.start, b = left.end;
            BigFloat c = right.start, d = right.end;

            BigFloat[] products = { a * c, a * d, b * c, b * d };

            BigFloat min = products.Min();
            BigFloat max = products.Max();

            return new Interval(min, max);
        }

        public static Interval operator /(Interval left, Interval right)
        {
            // Division by an interval containing zero is undefined
            if (right.Contains(0))
                throw new DivideByZeroException("Division by an interval containing zero is undefined.");

            BigFloat newRightA = 1 / right.End;
            BigFloat newRightB = 1 / right.Start;

            Interval newRight = new Interval(newRightA, newRightB);
            return left * newRight;
        }

        public bool ContainsPositive()
        {
            return end > 0;
        }

        public bool ContainsNegative()
        {
            return start < 0;
        }


        //  ZAŁOŻENIA:
        //  Funkcja sinus i cosinus są na danym przedziale monotoniczne

        public Interval Sin()
        {
            if (Start > End)
                throw new ArgumentException("Invalid interval");

            // Normalize to [0, 2π] for better behavior
            BigFloat twoPi = 2 * Math.PI;
            BigFloat a = Start % twoPi;
            BigFloat b = End % twoPi;

            if (a > b)
                b += twoPi;

            BigFloat sinA = BigFloat.Sin(a);
            BigFloat sinB = BigFloat.Sin(b);

            BigFloat min = Utility.Min(sinA, sinB);
            BigFloat max = Utility.Max(sinA, sinB);

            // Check for full sine wave within the interval
            if ((a <= Math.PI / 2 && b >= Math.PI / 2) ||
                (a <= 5 * Math.PI / 2 && b >= 5 * Math.PI / 2))
                max = 1;
            if ((a <= 3 * Math.PI / 2 && b >= 3 * Math.PI / 2) ||
                (a <= 7 * Math.PI / 2 && b >= 7 * Math.PI / 2))
                min = -1;

            return new Interval(min, max);
        }


        public Interval Cos()
        {
            if (Start > End)
                throw new ArgumentException("Invalid interval");

            BigFloat twoPi = 2 * Math.PI;

            // Normalize [a, b] to within [0, 2π]
            BigFloat a = Start % twoPi;
            BigFloat b = End % twoPi;
            if (a < 0) a += twoPi;
            if (b < 0) b += twoPi;
            if (a > b) b += twoPi; // to preserve order

            BigFloat cosA = BigFloat.Cos(a);
            BigFloat cosB = BigFloat.Cos(b);

            BigFloat min = Utility.Min(cosA, cosB);
            BigFloat max = Utility.Max(cosA, cosB);

            // Check if max occurs in [a, b] → at x = 0, 2π, etc.
            if ((a <= 0 && b >= 0) || (a <= twoPi && b >= twoPi))
                max = 1;

            // Check if min occurs in [a, b] → at x = π, 3π, etc.
            if ((a <= Math.PI && b >= Math.PI) || (a <= 3 * Math.PI && b >= 3 * Math.PI))
                min = -1;

            return new Interval(min, max);
        }


        public Interval Exp()
        {

            Interval e = new Interval(1);
            Interval w = e;

            int k = 1;
            bool finished = false;

            while (!finished && k < int.MaxValue / 2)
            {
                Interval d = new Interval(k);
                e = e * (this / d);
                Interval w1 = w + e;

                if (HasConverged(w, w1))
                {
                    return w1;
                }

                w = w1;
                k++;
            }

            throw new InvalidOperationException("Interval.Exp did not converge.");
        }

        public Interval Sqr()
        {

            BigFloat minx;
            BigFloat maxx;

            // Find minimum square
            if (start <= 0 && end >= 0)
            {
                // Interval contains 0
                minx = 0;
            }
            else if (start > 0)
            {
                minx = start;
            }
            else
            {
                minx = end;
            }

            // Find maximum square (based on absolute value)
            maxx = Utility.Max(BigFloat.Abs(start), BigFloat.Abs(end));

            BigFloat minSqr = minx * minx;
            BigFloat maxSqr = maxx * maxx;

            return new Interval(minSqr, maxSqr);
        }

        public Interval Sqrt()
        {
            if (start < 0)
                throw new ArgumentException("Cannot take square root of interval containing negative values.");
            return new Interval(BigFloat.Sqrt(start), BigFloat.Sqrt(end));
        }

        public Interval SqrtN(int n)
        {
            if (n <= 0)
                throw new ArgumentException("n must be positive.");
            return new Interval(BigFloat.Pow(start, 1.0 / n), BigFloat.Pow(end, 1.0 / n));
        }





        private static bool HasConverged(Interval prev, Interval next)
        {
            BigFloat eps = 1e-64;
            return RelDiff(prev.start, next.start) < eps &&
                   RelDiff(prev.end, next.end) < eps;
        }

        private static BigFloat RelDiff(BigFloat a, BigFloat b)
        {
            if (a == 0.0) return BigFloat.Abs(b);
            return BigFloat.Abs(a - b) / BigFloat.Abs(a);
        }

        public static readonly Interval Sqrt2 = new Interval(1.414213562373095048, 1.414213562373095049);
        public static readonly Interval Sqrt3 = new Interval(1.732050807568877293, 1.732050807568877294);
        public static readonly Interval Sqrt5 = new Interval(2.236067977499789696, 2.236067977499789697);
        public static readonly Interval Sqrt6 = new Interval(2.449489742783178098, 2.449489742783178099);
        public static readonly Interval Sqrt7 = new Interval(2.645751311064590590, 2.645751311064590591);
        public static readonly Interval Sqrt8 = new Interval(2.828427124746190097, 2.828427124746190098);
        public static readonly Interval Sqrt10 = new Interval(3.162277660168379331, 3.162277660168379332);
        public static readonly Interval Pi = new Interval(3.141592653589793238, 3.141592653589793239);






    }
}