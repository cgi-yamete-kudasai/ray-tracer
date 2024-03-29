using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Tests.Shapes;

public static class IntersectionResultTestHelper
{
    public static bool IsCorrect(this IntersectionResult intersectionResult) =>
        intersectionResult.T >= 0
        && Math.Abs(intersectionResult.Normal.Length() - 1) < 1e-6;

    public static bool IsDefault(this IntersectionResult intersectionResult) =>
        intersectionResult.T == 0
        && intersectionResult.Normal.Equals(Vector3.Zero)
        && intersectionResult.Point.Equals(Vector3.Zero);
}