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

    public static float[,] CreateRotationMatrix3x3(float xAngle, float yAngle, float zAngle, bool clockwise)
    {
        float xRad = DegToRad(xAngle) * (clockwise ? -1 : 1);
        float yRad = DegToRad(yAngle) * (clockwise ? -1 : 1);
        float zRad = DegToRad(zAngle) * (clockwise ? -1 : 1);

        float[,] rotationMatrixX = new float[3, 3]
        {
            { 1, 0, 0 },
            { 0, (float)Math.Cos(xRad), (float)-Math.Sin(xRad) },
            { 0, (float)Math.Sin(xRad), (float)Math.Cos(xRad) }
        };

        float[,] rotationMatrixY = new float[3, 3]
        {
            { (float)Math.Cos(yRad), 0, (float)Math.Sin(yRad) },
            { 0, 1, 0 },
            { (float)-Math.Sin(yRad), 0, (float)Math.Cos(yRad) }
        };

        float[,] rotationMatrixZ = new float[3, 3]
        {
            { (float)Math.Cos(zRad), (float)-Math.Sin(zRad), 0 },
            { (float)Math.Sin(zRad), (float)Math.Cos(zRad), 0 },
            { 0, 0, 1 }
        };

        float[,] rotationMatrix = MultiplyByMatrix(rotationMatrixX, rotationMatrixY);
        rotationMatrix = MultiplyByMatrix(rotationMatrix, rotationMatrixZ);

        return rotationMatrix;
    }

    public static float[,] CreateRotationMatrix4x4(float xAngle, float yAngle, float zAngle, bool clockwise)
    {
        float xRad = DegToRad(xAngle) * (clockwise ? -1 : 1);
        float yRad = DegToRad(yAngle) * (clockwise ? -1 : 1);
        float zRad = DegToRad(zAngle) * (clockwise ? -1 : 1);

        float[,] rotationMatrixX = new float[4, 4]
        {
            { 1, 0, 0, 0 },
            { 0, (float)Math.Cos(xRad), (float)-Math.Sin(xRad), 0 },
            { 0, (float)Math.Sin(xRad), (float)Math.Cos(xRad), 0 },
            { 0, 0, 0, 1 }
        };

        float[,] rotationMatrixY = new float[4, 4]
        {
            { (float)Math.Cos(yRad), 0, (float)Math.Sin(yRad), 0 },
            { 0, 1, 0, 0 },
            { (float)-Math.Sin(yRad), 0, (float)Math.Cos(yRad), 0 },
            { 0, 0, 0, 1 }
        };

        float[,] rotationMatrixZ = new float[4, 4]
        {
            { (float)Math.Cos(zRad), (float)-Math.Sin(zRad), 0, 0 },
            { (float)Math.Sin(zRad), (float)Math.Cos(zRad), 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        float[,] rotationMatrix = MultiplyByMatrix(rotationMatrixX, rotationMatrixY);
        rotationMatrix = MultiplyByMatrix(rotationMatrix, rotationMatrixZ);

        return rotationMatrix;
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
    
    

    public static Vector3 MultiplyByVector3(this float[,] matrix, Vector3 vector)
    {
        if (matrix.GetLength(1) == 3)
        {
            float x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z;
            float y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            float z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z;
            return new Vector3(x, y, z);
        }

        if (matrix.GetLength(1) == 4)
        {
            float x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z + matrix[0, 3];
            float y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z + matrix[1, 3];
            float z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z + matrix[2, 3];
            return new Vector3(x, y, z);
        }

        throw new ArgumentException("Matrix cannot be multiplied by vector");
    }

    public static Vector3 Transform(this Vector3 vector, float[,] matrix)
    {
        return matrix.MultiplyByVector3(vector);
    }
}