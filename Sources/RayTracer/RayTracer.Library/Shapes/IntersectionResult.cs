using System;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public readonly ref struct IntersectionResult
{
    public readonly Vector3 Point;

    public readonly float T;
    
    public readonly Vector3 Normal;

    public IntersectionResult(Vector3 point, float t, Vector3 normal)
    {
        Point = point;
        T = t;
        
        // Assert.True(Vector3.IsUnit(normal)); Too small tolerance
        Normal = normal;
    }
}
