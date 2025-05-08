using System;
using Numerics.NET;

namespace Intervals;

public class Utility
{

    public static BigFloat Min(BigFloat a, BigFloat b) {
        if(a > b)
            return b;

        return a;
    }

    public static BigFloat Max(BigFloat a, BigFloat b) {
        if(a > b)
            return a;

        return b;
    }

}
