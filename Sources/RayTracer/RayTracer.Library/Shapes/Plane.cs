using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Shapes;

public class Plane : IIntersectable, ISerializable<Plane>
{
    public Vector3 BasePoint { get; private set; }

    public Vector3 Normal { get; private set; }

    public Plane(Vector3 basePoint, Vector3 normal)
    {
        BasePoint = basePoint;
        Normal = Vector3.Normalize(normal);
    }

    public BoundingBox BoundingBox => throw new NotImplementedException();

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

    public void Transform(WorldTransform wt)
    {
        BasePoint = BasePoint.Transform(wt);
        Normal = Normal.Transform(wt);
    }

    public static ISerializer<Plane> Serializer => PlaneSerializer.Instance;
}
