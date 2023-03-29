using System;

namespace RayTracer.Library.Extensions;

public static class SingleExtensions
{
    private const int TOLERANCE = 5;

    public static unsafe bool IsEqualTo(this float lhs, float rhs)
    {
        int castLhs = *(int*)&lhs;
        int castRhs = *(int*)&rhs;

        if (castLhs >> 31 != castRhs >> 31)
            return lhs == rhs;

        return Math.Abs(castLhs - castRhs) <= TOLERANCE;
    }
}
