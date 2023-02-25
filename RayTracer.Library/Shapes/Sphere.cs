using System;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Sphere : IIntersectable
{
    public Vector3 Center { get; init; }

    public float Radius { get; init; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool TryIntersect(in IntersectionContext context, out ColorRGB color)
    {
        ref readonly Ray ray = ref context.Ray;
        ref readonly Vector3 light = ref context.DirectionalLight;

        Vector3 k = ray.Origin - Center;
        float a = Vector3.Dot(ray.Direction, ray.Direction);
        float b = 2 * Vector3.Dot(k, ray.Direction);
        float c = Vector3.Dot(k, k) - Radius * Radius;

        var (root1, root2) = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);

        if (rootsCount == 0)
        {
            color = new ColorRGB(0);
            return false;
        }

        float closest = Math.Min(root1, root2);
        Vector3 intersectionPoint = ray.Origin + closest * ray.Direction;
        Vector3 normal = intersectionPoint - Center;

        float dot = Vector3.Dot(normal, -1 * light);
        color = new ColorRGB(dot);
        return true;
    }
}
