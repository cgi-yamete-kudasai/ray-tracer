using System;
using System.Diagnostics;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class TriangleMesh : IIntersectable
{
    public Vector3 A { get; private set; }
    public Vector3 B { get; private set; }
    public Vector3 C { get; private set; }

    public Vector3 NormalA { get; private set; }
    public Vector3 NormalB { get; private set; }
    public Vector3 NormalC { get; private set; }

    public TriangleMesh(Vector3 a, Vector3 b, Vector3 c, Vector3 normalA, Vector3 normalB, Vector3 normalC)
    {
        A = a;
        B = b;
        C = c;

        Debug.Assert(Vector3.IsUnit(normalA));
        Debug.Assert(Vector3.IsUnit(normalB));
        Debug.Assert(Vector3.IsUnit(normalC));

        NormalA = normalA;
        NormalB = normalB;
        NormalC = normalC;
    }

    public BoundingBox BB => CalculateBoundingBox();

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

        result = new(point, distance, CalculateNormalBarycentric(point));
        return true;
    }

    public void Transform(WorldTransform wt)
    {
        A = A.Transform(wt);
        B = B.Transform(wt);
        C = C.Transform(wt);

        NormalA = NormalA.Transform(wt);
        NormalB = NormalB.Transform(wt);
        NormalC = NormalC.Transform(wt);
    }

    private Vector3 CalculateNormalBarycentric(Vector3 p)
    {
        var vector0 = B - A;
        var vector1 = C - A;
        var vector2 = p - A;
        var dot00 = Vector3.Dot(vector0, vector0);
        var dot01 = Vector3.Dot(vector0, vector1);
        var dot11 = Vector3.Dot(vector1, vector1);
        var dot20 = Vector3.Dot(vector2, vector0);
        var dot21 = Vector3.Dot(vector2, vector1);

        var denom = dot00 * dot11 - dot01 * dot01;

        var v = (dot11 * dot20 - dot01 * dot21) / denom;
        var w = (dot00 * dot21 - dot01 * dot20) / denom;
        var u = 1 - v - w;

        return u * NormalA + v * NormalB + w * NormalC;
    }

    private Vector3 CalculateNormalNaive(Vector3 point)
    {
        float distanceA = Vector3.Distance(A, point);
        float distanceB = Vector3.Distance(B, point);
        float distanceC = Vector3.Distance(C, point);

        float weightA = 1 / distanceA;
        float weightB = 1 / distanceB;
        float weightC = 1 / distanceC;

        float totalWeight = weightA + weightB + weightC;

        return Vector3.Normalize(
            weightA / totalWeight * NormalA
            + weightB / totalWeight * NormalB
            + weightC / totalWeight * NormalC);
    }

    private BoundingBox CalculateBoundingBox()
    {
        float minX = MathF.Min(A.X, MathF.Min(B.X, C.X));
        float minY = MathF.Min(A.Y, MathF.Min(B.Y, C.Y));
        float minZ = MathF.Min(A.Z, MathF.Min(B.Z, C.Z));

        float maxX = MathF.Max(A.X, MathF.Max(B.X, C.X));
        float maxY = MathF.Max(A.Y, MathF.Max(B.Y, C.Y));
        float maxZ = MathF.Max(A.Z, MathF.Max(B.Z, C.Z));

        return new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
    }
}