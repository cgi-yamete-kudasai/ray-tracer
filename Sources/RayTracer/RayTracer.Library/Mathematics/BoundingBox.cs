using System;
using RayTracer.Library.IIntersectableTrees.OctTrees;

namespace RayTracer.Library.Mathematics;

public struct BoundingBox
{
    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }

    public BoundingBox(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }
    
    public static BoundingBox Zero => new(Vector3.Zero, Vector3.Zero);

    public static BoundingBox Union(BoundingBox bb1, BoundingBox bb2)
    {
        float minX = MathF.Min(bb1.Min.X, bb2.Min.X);
        float minY = MathF.Min(bb1.Min.Y, bb2.Min.Y);
        float minZ = MathF.Min(bb1.Min.Z, bb2.Min.Z);
        
        float maxX = MathF.Max(bb1.Max.X, bb2.Max.X);
        float maxY = MathF.Max(bb1.Max.Y, bb2.Max.Y);
        float maxZ = MathF.Max(bb1.Max.Z, bb2.Max.Z);
        
        return new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
    }

    public float Size {
        get
        {
            Vector3 size = Max - Min;
            return MathF.Sqrt(size.X * size.X + size.Y * size.Y + size.Z * size.Z);
        }
    }

    public ContainmentType Contains(BoundingBox anotherBb)
    {
        if (Min.X <= anotherBb.Min.X && Min.Y <= anotherBb.Min.Y && Min.Z <= anotherBb.Min.Z
            && Max.X >= anotherBb.Max.X && Max.Y >= anotherBb.Max.Y && Max.Z >= anotherBb.Max.Z)
        {
            return ContainmentType.Contains;
        }

        return ContainmentType.Disjoint;
    }

    public bool IntersectsWithRay(in Ray ray, out float t)
    {
        float tmin = (Min.X - ray.Origin.X) / ray.Direction.X;
        float tmax = (Max.X - ray.Origin.X) / ray.Direction.X;
        t = 0;
        
        if (tmin > tmax)
        {
            (tmin, tmax) = (tmax, tmin);
        }

        float tymin = (Min.Y - ray.Origin.Y) / ray.Direction.Y;
        float tymax = (Max.Y - ray.Origin.Y) / ray.Direction.Y;

        if (tymin > tymax)
        {
            (tymin, tymax) = (tymax, tymin);
        }

        if ((tmin > tymax) || (tymin > tmax))
        {
            t = -1;
            return false;
        }

        if (tymin > tmin)
            tmin = tymin;

        if (tymax < tmax)
            tmax = tymax;

        float tzmin = (Min.Z - ray.Origin.Z) / ray.Direction.Z;
        float tzmax = (Max.Z - ray.Origin.Z) / ray.Direction.Z;

        if (tzmin > tzmax)
        {
            (tzmin, tzmax) = (tzmax, tzmin);
        }

        if ((tmin > tzmax) || (tzmin > tmax))
        {
            t = -1;
            return false;
        }

        if (tzmin > tmin)
            tmin = tzmin;

        if (tzmax < tmax)
            tmax = tzmax;

        t = tmin;

        if (tmax < tmin || tmax < 0)
        {
            t = -1;
            return false;
        }
        return true;
    }

}