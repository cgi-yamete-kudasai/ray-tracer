using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Shapes;

public class Sphere : IIntersectable, ISerializable<Sphere>
{
    public Vector3 Center { get; private set; }

    public float Radius { get; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public BoundingBox BB => new(Center - new Vector3(Radius), Center + new Vector3(Radius));

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        Vector3 k = ray.Origin - Center;
        float a = Vector3.Dot(ray.Direction, ray.Direction);
        float b = 2 * Vector3.Dot(k, ray.Direction);
        float c = Vector3.Dot(k, k) - Radius * Radius;

        var (root1, root2) = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);

        if (b >= 0 || rootsCount == 0)
        {
            result = default;
            return false;
        }

        float closest = Math.Min(root1, root2);
        Vector3 point = ray.Origin + closest * ray.Direction;

        result = new(point, closest, GetNormal(point));
        return true;
    }

    public void Transform(WorldTransform wt)
    {
        Center = Center.Transform(wt);
    }

    private Vector3 GetNormal(in Vector3 point)
    {
        return Vector3.Normalize(point - Center);
    }

    static ISerializer<Sphere> ISerializable<Sphere>.Serializer => SphereSerializer.Instance;
}