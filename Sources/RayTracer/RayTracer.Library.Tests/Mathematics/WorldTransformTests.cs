using System;
using System.Numerics;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using Vector3 = RayTracer.Library.Mathematics.Vector3;

namespace RayTracer.Library.Tests.Mathematics;

public class WorldTransformTests
{
    public const float TOLERANCE = 0.0001f;
    private readonly ITestOutputHelper _testOutputHelper;


    public WorldTransformTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void WorldTransformRotationX()
    {
        float xAngle = 90;
        bool clockwise = false;

        float[,] expected = new float[4, 4]
        {
            { 1, 0, 0, 0 },
            { 0, 0, -1, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateX(xAngle, clockwise).Matrix;

        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        Assert.True(CompareMatrices(expected, actual));
    }

    [Fact]
    public void WorldTransformRotationY()
    {
        float yAngle = 90;
        bool clockwise = false;

        float[,] expected = new float[4, 4]
        {
            { 0, 0, 1, 0 },
            { 0, 1, 0, 0 },
            { -1, 0, 0, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateY(yAngle, clockwise).Matrix;

        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        Assert.True(CompareMatrices(expected, actual));
    }


    [Fact]
    public void WorldTransformRotationZ()
    {
        float zAngle = 90;
        bool clockwise = false;

        float[,] expected = new float[4, 4]
        {
            { 0, -1, 0, 0 },
            { 1, 0, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] actual = WorldTransform.Identity.RotateZ(zAngle, clockwise).Matrix;

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

        Vector3 start = new Vector3(1, 0, 1);
        Vector3 expected = new Vector3(1.353553391f,	0.3535533906f,	-0.2071067812f);

        WorldTransform transform = WorldTransform.Identity
                .RotateX(xAngle, clockwise) 
                .RotateY(yAngle, clockwise)
             .RotateZ(zAngle, clockwise);
        Vector3 actual =
            transform.ApplyTransform(start);

       Assert.True(CompareVectors(expected, actual));
    }

    [Fact]
    public void WorldTransformScale()
    {
        Vector3 scale = new Vector3(2, 3, 4);
        Vector3 start = new Vector3(-4, 6, 8);
        
        Vector3 expected = new Vector3(-8, 18, 32);
        Assert.True(CompareVectors(expected, WorldTransform.Identity.Scale(scale).ApplyTransform(start)));
    }
    
    [Fact]
    public void WorldTransformTranslate()
    {
        Vector3 translation = new Vector3(5, -3, 2);
        Vector3 start = new Vector3(-3, 4, 5);
        
        Vector3 expected = new Vector3(2, 1, 7);
        Assert.True(CompareVectors(expected, WorldTransform.Identity.Translate(translation).ApplyTransform(start)));
    }

    private bool CompareVectors(Vector3 expected, Vector3 actual)
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
                //areEqual &= expected[i, j].IsEqualTo(actual[i, j]);
            }
        }

        return areEqual;
    }
}