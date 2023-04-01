using System;

namespace RayTracer.Library.Mathematics;

public readonly struct WorldTransform
{
    public static readonly WorldTransform Identity = new(new float[,]
    {
        { 1, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
    });

    public float[,] Matrix { get; }

    private WorldTransform(float[,] matrix)
    {
        Matrix = matrix;
    }

    public WorldTransform RotateX(float rad, bool clockwise = true)
    {
        if (clockwise)
            rad *= -1;

        float[,] rotationMatrix =
        {
            { 1, 0, 0, 0 },
            { 0, (float)Math.Cos(rad), -(float)Math.Sin(rad), 0 },
            { 0, (float)Math.Sin(rad), (float)Math.Cos(rad), 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(MathHelper.MultiplyMatrices(rotationMatrix, Matrix));
    }
    
    public WorldTransform RotateY(float rad, bool clockwise = true)
    {
        if (clockwise)
            rad *= -1;

        float[,] rotationMatrix =
        {
            { (float)Math.Cos(rad), 0, (float)Math.Sin(rad), 0 },
            { 0, 1, 0, 0 },
            { -(float)Math.Sin(rad), 0, (float)Math.Cos(rad), 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(MathHelper.MultiplyMatrices(rotationMatrix, Matrix));
    }
    
    public WorldTransform RotateZ(float rad, bool clockwise = true)
    {
        if (clockwise)
            rad *= -1;

        float[,] rotationMatrix =
        {
            { (float)Math.Cos(rad), -(float)Math.Sin(rad), 0, 0 },
            { (float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(MathHelper.MultiplyMatrices(rotationMatrix, Matrix));
    }
}
