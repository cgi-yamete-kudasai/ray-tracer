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

    public static float[,] CreateRotationMatrix4x4(float xAngle, float yAngle, float zAngle, bool clockwise)
    {
        double xRad = xAngle / 180 * Math.PI * (clockwise ? -1 : 1);
        double yRad = yAngle / 180 * Math.PI * (clockwise ? -1 : 1);
        double zRad = zAngle / 180 * Math.PI * (clockwise ? -1 : 1);

        double[,] rotationMatrixX = new double[4, 4]
        {
            { 1, 0, 0, 0 },
            { 0, Math.Cos(xRad), -Math.Sin(xRad), 0 },
            { 0, Math.Sin(xRad), Math.Cos(xRad), 0 },
            { 0, 0, 0, 1 }
        };

        double[,] rotationMatrixY = new double[4, 4]
        {
            { Math.Cos(yRad), 0, Math.Sin(yRad), 0 },
            { 0, 1, 0, 0 },
            { -Math.Sin(yRad), 0, Math.Cos(yRad), 0 },
            { 0, 0, 0, 1 }
        };

        double[,] rotationMatrixZ = new double[4, 4]
        {
            { Math.Cos(zRad), -Math.Sin(zRad), 0, 0 },
            { Math.Sin(zRad), Math.Cos(zRad), 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] rotationMatrix = MultiplyByMatrix(rotationMatrixX.ToFloat(), rotationMatrixY.ToFloat());
        rotationMatrix = MultiplyByMatrix(rotationMatrix, rotationMatrixZ.ToFloat());

        return rotationMatrix;
    }

    public static float[,] ToFloat(this double[,] matrix)
    {
        float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                result[i, j] = (float)matrix[i, j];
            }
        }

        return result;
    }

    public static float[,] MultiplyByMatrix(this float[,] matrix1, float[,] matrix2)
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

    public static Vector3 Transform(this Vector3 vector, float[,] transformMatrix)
    {
        if (transformMatrix.GetLength(1) != 4)
        {
            throw new ArgumentException("Transform matrix must be 4x4");
        }

        float x = transformMatrix[0, 0] * vector.X + transformMatrix[0, 1] * vector.Y + transformMatrix[0, 2] * vector.Z + transformMatrix[0, 3];
        float y = transformMatrix[1, 0] * vector.X + transformMatrix[1, 1] * vector.Y + transformMatrix[1, 2] * vector.Z + transformMatrix[1, 3];
        float z = transformMatrix[2, 0] * vector.X + transformMatrix[2, 1] * vector.Y + transformMatrix[2, 2] * vector.Z + transformMatrix[2, 3];
        return new Vector3(x, y, z);
    }
}