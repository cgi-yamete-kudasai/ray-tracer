using System;
using RayTracer.Library.Extensions;

namespace RayTracer.Library.Mathematics;

public partial struct Vector3
{
    public static readonly Vector3 Zero = new(0, 0, 0);

    public static float Dot(in Vector3 lhs, in Vector3 rhs)
    {
        return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
    }

    public static Vector3 Cross(in Vector3 lhs, in Vector3 rhs)
    {
        return new(lhs.Y * rhs.Z - rhs.Y * lhs.Z, rhs.X * lhs.Z - lhs.X * rhs.Z, lhs.X * rhs.Y - rhs.X * lhs.Y);
    }

    public static Vector3 Normalize(in Vector3 vector)
    {
        float coefficient = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        return new(vector.X / coefficient, vector.Y / coefficient, vector.Z / coefficient);
    }

    public static bool IsUnit(in Vector3 vector)
    {
        return vector.LengthSquared().IsEqualTo(1);
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

    public static Vector3 operator /(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    }

    public static float Distance(in Vector3 lhs, in Vector3 rhs)
    {
        return MathF.Sqrt((lhs.X - rhs.X) * (lhs.X - rhs.X) +
                          (lhs.Y - rhs.Y) * (lhs.Y - rhs.Y) +
                          (lhs.Z - rhs.Z) * (lhs.Z - rhs.Z));
    }
    
    public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
    {
        return ((1f - amount) * value1) + (amount * value2);
    }
}