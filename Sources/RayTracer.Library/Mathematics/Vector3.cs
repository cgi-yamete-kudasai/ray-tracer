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
    
    public float Length() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector3 other && other.X.IsEqualTo(X) && other.Y.IsEqualTo(Y) && other.Z.IsEqualTo(Z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    static ISerializer<Vector3> ISerializable<Vector3>.Serializer => Vector3Serializer.Instance;
}
