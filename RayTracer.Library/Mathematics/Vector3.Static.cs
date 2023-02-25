using System;

namespace RayTracer.Library.Mathematics;

public partial struct Vector3
{
    public static readonly Vector3 Zero = new(0, 0, 0);

    public static float Dot(in Vector3 lhs, in Vector3 rhs)
    {
        return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
    }

    public static Vector3 Normalize(in Vector3 vector)
    {
        float coefficient = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        return new Vector3(vector.X / coefficient, vector.Y / coefficient, vector.Z / coefficient);
    }

    public static Vector3 operator +(in Vector3 lhs, in Vector3 rhs)
    {
        return new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3 operator -(in Vector3 lhs, in Vector3 rhs)
    {
        return new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Vector3 operator *(float lhs, in Vector3 rhs)
    {
        return new(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs);
    }
}
