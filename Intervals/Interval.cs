

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
            start = number;
            end = number;
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
            return $"{start}...{end}";
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
            double c = right.start, d = right.end;

            // Division by an interval containing zero is undefined
            if (right.Contains(0))
                throw new DivideByZeroException("Division by an interval containing zero is undefined.");

            double a = left.start, b = left.end;

            // Compute all 4 combinations
            double[] quotients =
            {
            a / c, a / d,
            b / c, b / d
        };

            double min = quotients.Min();
            double max = quotients.Max();

            return new Interval(min, max);
        }

        public Interval Sin()
        {

            Interval s = this;
            Interval w = this;
            Interval x2 = this * this;

            int k = 1;
            bool isEven = true;
            bool finished = false;

            while (!finished && k < int.MaxValue / 2)
            {
                double denom = (k + 1) * (k + 2);
                Interval d = new Interval(denom);

                s = s * (x2 / d);

                Interval w1 = isEven ? w - s : w + s;

                if (HasConverged(w, w1))
                {
                    // Clamp result to [-1, 1] due to sin() range
                    double a = Math.Max(-1, w1.start);
                    double b = Math.Min(1, w1.end);
                    return new Interval(a, b);
                }

                w = w1;
                k += 2;
                isEven = !isEven;
            }

            throw new InvalidOperationException("Interval.Sin did not converge.");
        }

        public Interval Cos()
        {

            Interval c = new Interval(1);
            Interval w = c;
            Interval x2 = this * this;

            int k = 1;
            bool isEven = true;
            bool finished = false;

            while (!finished && k < int.MaxValue / 2)
            {
                double denom = k * (k + 1);
                Interval d = new Interval(denom);

                c = c * (x2 / d);

                Interval w1 = isEven ? w - c : w + c;

                if (HasConverged(w, w1))
                {
                    // Clamp result to [-1, 1]
                    Console.WriteLine($"{w1.Start} {w1.End}");
                    double a = Math.Max(-1, w1.start);
                    double b = Math.Min(1, w1.end);
                    return new Interval(a, b);
                }

                w = w1;
                k += 2;
                isEven = !isEven;
            }

            throw new InvalidOperationException("Interval.Cos did not converge.");
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
            const double eps = 1e-18;
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