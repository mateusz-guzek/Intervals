using System;
using Intervals;
using Numerics.NET;
using Interval = Intervals.Interval;

namespace Nonlinear_Solvers;

// public static class Function
// {
//     
//     public static double Eval(double x) {
//         return x;
//
//     }
//
//     public static Interval Eval(Interval x) {
//         return x;
//     }
//
//
//
// }

public interface Function
{
    public BigFloat Eval(BigFloat x);
    
    public Interval Eval(Interval x);

    public String StringRepresentation
    {
        get;
    }
}
