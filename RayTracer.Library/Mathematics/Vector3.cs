using System;
using System.Diagnostics;
using RayTracer.Library.Extensions;

namespace RayTracer.Library.Mathematics;

[DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
public readonly partial struct Vector3
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

    public override bool Equals(object? obj)
    {
        return obj is Vector3 other && other.X.IsEqualTo(X) && other.Y.IsEqualTo(Y) && other.Z.IsEqualTo(Z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
