using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Extensions;

public static class RotationExtensions
{
    public static Vector3 Transform(this in Vector3 vector, WorldTransform wt)
    {
        float x = wt.Matrix[0, 0] * vector.X + wt.Matrix[0, 1] * vector.Y + wt.Matrix[0, 2] * vector.Z + wt.Matrix[0, 3];
        float y = wt.Matrix[1, 0] * vector.X + wt.Matrix[1, 1] * vector.Y + wt.Matrix[1, 2] * vector.Z + wt.Matrix[1, 3];
        float z = wt.Matrix[2, 0] * vector.X + wt.Matrix[2, 1] * vector.Y + wt.Matrix[2, 2] * vector.Z + wt.Matrix[2, 3];
        
        return new(x, y, z);
    }
}
