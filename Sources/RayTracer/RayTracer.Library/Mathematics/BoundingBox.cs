using System;

namespace RayTracer.Library.Mathematics;

public struct BoundingBox
{
    public Vector3 MinPoint { get; private set; }
    public Vector3 MaxPoint { get; private set; }

    public BoundingBox(Vector3 minPoint, Vector3 maxPoint)
    {
        MinPoint = minPoint;
        MaxPoint = maxPoint;
    }
    
    public static BoundingBox Zero => new(Vector3.Zero, Vector3.Zero);

    public static BoundingBox Union(BoundingBox bb1, BoundingBox bb2)
    {
        float minX = MathF.Min(bb1.MinPoint.X, bb2.MinPoint.X);
        float minY = MathF.Min(bb1.MinPoint.Y, bb2.MinPoint.Y);
        float minZ = MathF.Min(bb1.MinPoint.Z, bb2.MinPoint.Z);
        
        float maxX = MathF.Max(bb1.MaxPoint.X, bb2.MaxPoint.X);
        float maxY = MathF.Max(bb1.MaxPoint.Y, bb2.MaxPoint.Y);
        float maxZ = MathF.Max(bb1.MaxPoint.Z, bb2.MaxPoint.Z);
        
        return new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
    }
}