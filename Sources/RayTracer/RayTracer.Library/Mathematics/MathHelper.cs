using System;

namespace RayTracer.Library.Mathematics;

public static class MathHelper
{
    public static float DegToRad(float deg)
    {
        return (deg / 180) * (float)Math.PI;
    }

    public static float RadToDeg(float rad)
    {
        return (rad / (float)Math.PI) * 180;
    }

    public static (float, float) SolveQuadraticEquation(float a, float b, float c, out int rootsCount)
    {
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            rootsCount = 0;
            return (float.NaN, float.NaN);
        }

        if (discriminant == 0)
        {
            float root = -b / (2 * a);
            rootsCount = 1;
            return (root, root);
        }

        float root1 = (-b + (float)Math.Sqrt(discriminant)) / (2 * a);
        float root2 = (-b - (float)Math.Sqrt(discriminant)) / (2 * a);

        rootsCount = 2;
        return (root1, root2);
    }

    public static float[,] Multiply(this float[,] matrix1, float[,] matrix2)
    {
        if (matrix1.GetLength(1) != matrix2.GetLength(0))
        {
            throw new ArgumentException("Matrices cannot be multiplied");
        }

        float[,] result = new float[matrix1.GetLength(0), matrix2.GetLength(1)];
        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                for (int k = 0; k < matrix1.GetLength(1); k++)
                {
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }

        return result;
    }
}