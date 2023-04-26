using System;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Triangle : IIntersectable
{
    public Vector3 A { get; private set; }

    public Vector3 B { get; private set; }

    public Vector3 C { get; private set; }

    private readonly Lazy<Vector3> _normal;

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        A = a;
        B = b;
        C = c;

        _normal = new(() => FindNormal(a, b, c));
    }

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        result = default;

        // Möller–Trumbore intersection algorithm
        // https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm

        Vector3 orig = ray.Origin;
        Vector3 dir = ray.Direction;
        Vector3 v0 = A;
        Vector3 v1 = B;
        Vector3 v2 = C;

        Vector3 e1 = v1 - v0;
        Vector3 e2 = v2 - v0;

        Vector3 pvec = Vector3.Cross(dir, e2);

        float det = Vector3.Dot(e1, pvec);

        if (det < 1e-8 && det > -1e-8)
            return false;

        float inv_det = 1 / det;
        Vector3 tvec = orig - v0;
        float u = Vector3.Dot(tvec, pvec) * inv_det;

        if (u < 0 || u > 1)
            return false;

        Vector3 qvec = Vector3.Cross(tvec, e1);
        float v = Vector3.Dot(dir, qvec) * inv_det;

        if (v < 0 || u + v > 1)
            return false;

        float distance = Vector3.Dot(e2, qvec) * inv_det;

        if (distance < 0)
            return false;

        Vector3 point = ray.Origin + distance * ray.Direction;

        result = new(point, distance, _normal.Value);
        return true;
    }

    public void Transform(WorldTransform wt)
    {
        A = A.Transform(wt);
        B = B.Transform(wt);
        C = C.Transform(wt);
    }

    private static Vector3 FindNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 e1 = b - a;
        Vector3 e2 = c - a;

        return Vector3.Normalize(Vector3.Cross(e1, e2));
    }
}
