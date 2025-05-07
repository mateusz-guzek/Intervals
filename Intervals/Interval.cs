

using System.Reflection.Metadata;

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
    /// pi
    /// 
    /// 
    /// 
    public class Interval
    {

        private const double epsilon = 1e-15;

        private double start;
        private double end;

        public double Start => start;
        public double End => end;



        public Interval(double start, double end)
        {
            if (start.CompareTo(end) > 0)
                throw new ArgumentException("Start cannot be greater than end.");

            this.start = start;
            this.end = end;
        }

        public Interval(double number)
        {
            start = number - epsilon;
            end = number + epsilon;
        }

        public bool Contains(double number)
        {
            return start <= number && end >= number;
        }

        public double Width()
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
            double a = left.start, b = left.end;
            double c = right.start, d = right.end;

            double[] products = { a * c, a * d, b * c, b * d };

            double min = products.Min();
            double max = products.Max();

            return new Interval(min, max);
        }

        public static Interval operator /(Interval left, Interval right)
        {
            // Division by an interval containing zero is undefined
            if (right.Contains(0))
                throw new DivideByZeroException("Division by an interval containing zero is undefined.");

            double newRightA = 1 / right.End;
            double newRightB = 1 / right.Start;

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
            double twoPi = 2 * Math.PI;
            double a = Start % twoPi;
            double b = End % twoPi;

            if (a > b)
                b += twoPi;

            double sinA = Math.Sin(a);
            double sinB = Math.Sin(b);

            double min = Math.Min(sinA, sinB);
            double max = Math.Max(sinA, sinB);

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

            double twoPi = 2 * Math.PI;

            // Normalize [a, b] to within [0, 2π]
            double a = Start % twoPi;
            double b = End % twoPi;
            if (a < 0) a += twoPi;
            if (b < 0) b += twoPi;
            if (a > b) b += twoPi; // to preserve order

            double cosA = Math.Cos(a);
            double cosB = Math.Cos(b);

            double min = Math.Min(cosA, cosB);
            double max = Math.Max(cosA, cosB);

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

            double minx;
            double maxx;

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
            maxx = Math.Max(Math.Abs(start), Math.Abs(end));

            double minSqr = minx * minx;
            double maxSqr = maxx * maxx;

            return new Interval(minSqr, maxSqr);
        }

        public Interval Sqrt()
        {
            if (start < 0)
                throw new ArgumentException("Cannot take square root of interval containing negative values.");
            return new Interval(Math.Sqrt(start), Math.Sqrt(end));
        }

        public Interval SqrtN(int n)
        {
            if (n <= 0)
                throw new ArgumentException("n must be positive.");
            return new Interval(Math.Pow(start, 1.0 / n), Math.Pow(end, 1.0 / n));
        }





        private static bool HasConverged(Interval prev, Interval next)
        {
            const double eps = 1e-64;
            return RelDiff(prev.start, next.start) < eps &&
                   RelDiff(prev.end, next.end) < eps;
        }

        private static double RelDiff(double a, double b)
        {
            if (a == 0.0) return Math.Abs(b);
            return Math.Abs(a - b) / Math.Abs(a);
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