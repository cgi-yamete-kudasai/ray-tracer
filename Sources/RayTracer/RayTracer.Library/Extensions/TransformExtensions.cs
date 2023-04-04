using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Extensions;

public static class TransformExtensions
{
    public static Vector3 Transform(this in Vector3 vector, WorldTransform wt)
    {
        float x = wt.Matrix[0, 0] * vector.X + wt.Matrix[0, 1] * vector.Y + wt.Matrix[0, 2] * vector.Z + wt.Matrix[0, 3];
        float y = wt.Matrix[1, 0] * vector.X + wt.Matrix[1, 1] * vector.Y + wt.Matrix[1, 2] * vector.Z + wt.Matrix[1, 3];
        float z = wt.Matrix[2, 0] * vector.X + wt.Matrix[2, 1] * vector.Y + wt.Matrix[2, 2] * vector.Z + wt.Matrix[2, 3];
        
        return new(x, y, z);
    }

    public static Sphere Transform(this Sphere sphere, WorldTransform wt)
    {
        Vector3 center = sphere.Center.Transform(wt);

        return new(center, sphere.Radius);
    }

    public static Triangle Transform(this Triangle triangle, WorldTransform wt)
    {
        Vector3 a = triangle.A.Transform(wt);
        Vector3 b = triangle.B.Transform(wt);
        Vector3 c = triangle.C.Transform(wt);

        return new(a, b, c);
    }
}
