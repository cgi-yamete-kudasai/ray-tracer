using System;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Plane : IIntersectable
{
    public Vector3 BasePoint { get; }
    public Vector3 Normal { get; }

    public Plane(Vector3 basePoint, Vector3 normal)
    {
        BasePoint = basePoint;
        Normal = Vector3.Normalize(normal);
    }

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        float denominator = Vector3.Dot(Normal, ray.Direction);

        if (MathF.Abs(denominator) > IntersectionHelper.INTERSECTION_TOLERANCE)
        {
            Vector3 vector = BasePoint - ray.Origin;
            float t = Vector3.Dot(vector, Normal) / denominator;

            if (t >= 0)
            {
                result = new(ray.Origin + t * ray.Direction, t, Normal);
                return true;
            }
        }

        result = default;
        return false;
    }
}