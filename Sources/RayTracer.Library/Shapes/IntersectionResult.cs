using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public readonly ref struct IntersectionResult
{
    public readonly Vector3 Point;

    public readonly float T;

    public IntersectionResult(Vector3 point, float t)
    {
        Point = point;
        T = t;
    }
}
