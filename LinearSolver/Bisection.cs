using System;
using Intervals;

namespace Nonlinear_Solvers;

public static class Bisection
{
    public static double Eval(Func<double, double> f, double a, double b, int mit, double epsilon)
    {

        if (f(a) * f(b) > 0)
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = Math.Abs(epsilon);

        int iterations = 0;

        while (Math.Abs(a - b) > epsilon)
        {
            iterations++;

            double mid = (a + b) / 2;
            if (Math.Abs(f(mid)) < epsilon)
                return mid;

            if(f(a) * f(mid) < 0) {
                b = mid;
            } else {
                a = mid;
            }

            if(iterations >= mit) {
                return mid;
            }

        }

        return (a+b)/2;

    }

    public static Interval Eval(Func<Interval, Interval> f, Interval a, Interval b, int mit, double epsilon)
    {


        if(!(f(a) * f(b)).ContainsNegative())
            throw new ArgumentException("Function doesnt change its sign between a and b");


        epsilon = Math.Abs(epsilon);

        int iterations = 0;

        while (Math.Abs(b.End - a.Start) > epsilon)
        {
            iterations++;

            Interval mid = (a + b) / new Interval(2);
            if (f(mid).Contains(0) && Math.Abs(mid.End - mid.Start) < epsilon)
                return mid;

            if((f(a) * f(mid)).ContainsNegative()) {
                b = mid;
            } else {
                a = mid;
            }

            if(iterations >= mit) {
                return mid;
            }

        }

        return (a + b) / new Interval(2);

    }



}
