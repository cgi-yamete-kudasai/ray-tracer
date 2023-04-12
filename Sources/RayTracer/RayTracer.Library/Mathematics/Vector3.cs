using System;
using System.Diagnostics;
using RayTracer.Library.Extensions;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Mathematics;

[DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
public readonly partial struct Vector3 : ISerializable<Vector3>
{
    public readonly float X;

    public readonly float Y;
    
    public readonly float Z;
    
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float Length() => (float)Math.Sqrt(LengthSquared());

    public float LengthSquared() => X * X + Y * Y + Z * Z;

    public override bool Equals(object? obj)
    {
        return obj is Vector3 other && other.X.IsEqualTo(X) && other.Y.IsEqualTo(Y) && other.Z.IsEqualTo(Z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public Vector3 Transform(WorldTransform wt)
    {
        float x = wt.Matrix[0, 0] * X + wt.Matrix[0, 1] * Y + wt.Matrix[0, 2] * Z + wt.Matrix[0, 3];
        float y = wt.Matrix[1, 0] * X + wt.Matrix[1, 1] * Y + wt.Matrix[1, 2] * Z + wt.Matrix[1, 3];
        float z = wt.Matrix[2, 0] * X + wt.Matrix[2, 1] * Y + wt.Matrix[2, 2] * Z + wt.Matrix[2, 3];
        
        return new(x, y, z);
    }

    static ISerializer<Vector3> ISerializable<Vector3>.Serializer => Vector3Serializer.Instance;
}
