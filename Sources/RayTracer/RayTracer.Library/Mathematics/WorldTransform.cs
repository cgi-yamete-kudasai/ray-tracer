using System;

namespace RayTracer.Library.Mathematics;

public readonly struct WorldTransform
{
    public static readonly WorldTransform Identity = new(new float[4, 4]
    {
        { 1, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
    });

    public float[,] Matrix => _matrix;

    private readonly float[,] _matrix;

    private WorldTransform(float[,] matrix)
    {
        _matrix = matrix;
    }

    public readonly WorldTransform RotateX(float angle, bool clockwise = true)
    {
        double rad = MathHelper.DegToRad(angle) * (clockwise ? -1 : 1);

        float[,] rotationMatrix = new float[4, 4]
        {
            { 1, 0, 0, 0 },
            { 0, (float)Math.Cos(rad), -(float)Math.Sin(rad), 0 },
            { 0, (float)Math.Sin(rad), (float)Math.Cos(rad), 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(rotationMatrix.Multiply(_matrix));
    }

    public readonly WorldTransform RotateY(float angle, bool clockwise = true)
    {
        double rad = MathHelper.DegToRad(angle) * (clockwise ? -1 : 1);

        float[,] rotationMatrix = new float[4, 4]
        {
            { (float)Math.Cos(rad), 0, (float)Math.Sin(rad), 0 },
            { 0, 1, 0, 0 },
            { -(float)Math.Sin(rad), 0, (float)Math.Cos(rad), 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(rotationMatrix.Multiply(_matrix));
    }

    public readonly WorldTransform RotateZ(float angle, bool clockwise = true)
    {
        double rad = MathHelper.DegToRad(angle) * (clockwise ? -1 : 1);

        float[,] rotationMatrix = new float[4, 4]
        {
            { (float)Math.Cos(rad), -(float)Math.Sin(rad), 0, 0 },
            { (float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(rotationMatrix.Multiply(_matrix));
    }

    public readonly WorldTransform Translate(Vector3 vector)
    {
        float[,] translationMatrix = new float[4, 4]
        {
            { 1, 0, 0, vector.X },
            { 0, 1, 0, vector.Y },
            { 0, 0, 1, vector.Z },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(translationMatrix.Multiply(_matrix));
    }

    public readonly WorldTransform Scale(Vector3 vector)
    {
        float[,] scaleMatrix = new float[4, 4]
        {
            { vector.X, 0, 0, 0 },
            { 0, vector.Y, 0, 0 },
            { 0, 0, vector.Z, 0 },
            { 0, 0, 0, 1 }
        };

        return new WorldTransform(scaleMatrix.Multiply(_matrix));
    }

    public Vector3 ApplyTransform(Vector3 vector)
    {
        float x = _matrix[0, 0] * vector.X + _matrix[0, 1] * vector.Y + _matrix[0, 2] * vector.Z + _matrix[0, 3];
        float y = _matrix[1, 0] * vector.X + _matrix[1, 1] * vector.Y + _matrix[1, 2] * vector.Z + _matrix[1, 3];
        float z = _matrix[2, 0] * vector.X + _matrix[2, 1] * vector.Y + _matrix[2, 2] * vector.Z + _matrix[2, 3];
        return new Vector3(x, y, z);
    }
}