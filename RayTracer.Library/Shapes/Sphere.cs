using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Shapes;

public class Sphere : IIntersectable, ISerializable<Sphere>
{
    public Vector3 Center { get; init; }

    public float Radius { get; init; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool TryIntersect(in Ray ray, out Vector3 point)
    {
        Vector3 k = ray.Origin - Center;
        float a = Vector3.Dot(ray.Direction, ray.Direction);
        float b = 2 * Vector3.Dot(k, ray.Direction);
        float c = Vector3.Dot(k, k) - Radius * Radius;

        var (root1, root2) = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);

        if (rootsCount == 0)
        {
            point = Vector3.Zero;
            return false;
        }

        float closest = Math.Min(root1, root2);
        point = ray.Origin + closest * ray.Direction;
        return true;
    }

    public Vector3 GetNormal(in Vector3 point)
    {
        return point - Center;
    }

    static ISerializer<Sphere> ISerializable<Sphere>.Serializer => SphereSerializer.Instance;
}
