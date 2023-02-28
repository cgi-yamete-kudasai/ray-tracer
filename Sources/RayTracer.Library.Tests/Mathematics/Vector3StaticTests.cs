using System;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Tests.Mathematics;

public class Vector3StaticTests
{
    [Fact]
    public void Plus()
    {
        var v1 = new Vector3(1, 2, 3);
        var v2 = new Vector3(4, 5, 6);
        var result = new Vector3(5, 7, 9);

        var actual = v1 + v2;

        Assert.Equal(result, actual);
    }

    [Fact]
    public void Minus()
    {
        var v1 = new Vector3(4, 5, 6);
        var v2 = new Vector3(1, 2, 3);
        var result = new Vector3(3, 3, 3);

        var actual = v1 - v2;

        Assert.Equal(result, actual);
    }

    [Fact]
    public void Normalization()
    {
        var v = new Vector3(1, 2, 3);
        var length = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        var result = new Vector3(v.X / length, v.Y / length, v.Z / length);

        var actual = Vector3.Normalize(v);

        Assert.Equal(result, actual);
    }
    
    [Fact]
    public void ZeroVectorNormalization()
    {
        var v = Vector3.Zero;
        var result = Vector3.Zero;

        var actual = Vector3.Normalize(v);

        Assert.Equal(result, actual);
    }
    
    [Fact]
    public void DotMultiplication()
    {
        var v1 = new Vector3(4, 5, 6);
        var v2 = new Vector3(1, 2, 3);
        var result = 32f;

        var actual = Vector3.Dot(v1, v2);

        Assert.Equal(result, actual);
    }
    
    
    [Fact]
    public void CrossMultiplication()
    {
        var v1 = new Vector3(1, 2, 3);
        var v2 = new Vector3(4, 5, 6);
        var result = new Vector3(-3, 6, -3);

        var actual = Vector3.Cross(v1, v2);

        Assert.Equal(result, actual);
    }
}