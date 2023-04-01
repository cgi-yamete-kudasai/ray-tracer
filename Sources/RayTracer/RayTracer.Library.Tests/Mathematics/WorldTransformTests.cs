using System;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using static RayTracer.Library.Mathematics.MathHelper;

namespace RayTracer.Library.Tests.Mathematics;

public class WorldTransformTests
{
    public const float TOLERANCE = 0.0001f;

    [Fact]
    public void WorldTransformRotationX()
    {
        float xAngle = 90;
        bool clockwise = false;

        float[,] expected =
        {
            { 1, 0, 0, 0 },
            { 0, 0, -1, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateX(DegToRad(xAngle), clockwise).Matrix;

        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        Assert.True(CompareMatrices(expected, actual));
    }

    [Fact]
    public void WorldTransformRotationY()
    {
        float yAngle = 90;
        bool clockwise = false;

        float[,] expected =
        {
            { 0, 0, 1, 0 },
            { 0, 1, 0, 0 },
            { -1, 0, 0, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateY(DegToRad(yAngle), clockwise).Matrix;

        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        Assert.True(CompareMatrices(expected, actual));
    }

    [Fact]
    public void WorldTransformRotationZ()
    {
        float zAngle = 90;
        bool clockwise = false;

        float[,] expected =
        {
            { 0, -1, 0, 0 },
            { 1, 0, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateZ(DegToRad(zAngle), clockwise).Matrix;

        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        Assert.True(CompareMatrices(expected, actual));
    }

    [Fact]
    public void WorldTransformApplyRotation()
    {
        float xAngle = 45;
        float yAngle = 45;
        float zAngle = 45;
        bool clockwise = false;

        Vector3 start = new(1, 0, 1);
        Vector3 expected = new(1.353553391f, 0.3535533906f, -0.2071067812f);

        WorldTransform transform = WorldTransform.Identity
            .RotateX(DegToRad(xAngle), clockwise)
            .RotateY(DegToRad(yAngle), clockwise)
            .RotateZ(DegToRad(zAngle), clockwise);

        Vector3 actual = start.Transform(transform);

        Assert.True(CompareVectors(expected, actual));
    }

    private static bool CompareVectors(Vector3 expected, Vector3 actual)
    {
        return Math.Abs(expected.X - actual.X) < TOLERANCE &&
               Math.Abs(expected.Y - actual.Y) < TOLERANCE &&
               Math.Abs(expected.Z - actual.Z) < TOLERANCE;
    }

    private static bool CompareMatrices(float[,] expected, float[,] actual)
    {
        bool areEqual = true;
        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                areEqual &= Math.Abs(expected[i, j] - actual[i, j]) < TOLERANCE;
            }
        }

        return areEqual;
    }
}
