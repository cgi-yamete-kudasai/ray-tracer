using System;

namespace RayTracer.Library.Mathematics;

public static class MathHelper
{
    public static (float, float) SolveQuadraticEquation(float a, float b, float c, out int rootsCount)
    {
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            rootsCount = 0;
            return (float.NaN, float.NaN);
        }

        if (discriminant == 0)
        {
            float root = -b / (2 * a);
            rootsCount = 1;
            return (root, root);
        }

        float root1 = (-b + (float)Math.Sqrt(discriminant)) / (2 * a);
        float root2 = (-b - (float)Math.Sqrt(discriminant)) / (2 * a);

        rootsCount = 2;
        return (root1, root2);
    }
}
