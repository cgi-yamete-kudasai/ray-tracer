using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Utils;

public readonly struct CameraSettings
{
    public static readonly CameraSettings Default = new(16f / 9f, 1920, MathHelper.DegToRad(90), 1, Vector3.Zero);

    public float AspectRatio { get; init; }

    public int ImageHeight { get; init; }

    public float VerticalFOV { get; init; }

    public float FocalLength { get; init; }

    public Vector3 Origin { get; init; }

    public CameraSettings(
        float aspectRatio,
        int imageHeight,
        float verticalFOV,
        float focalLength,
        in Vector3 origin)
    {
        AspectRatio = aspectRatio;
        ImageHeight = imageHeight;
        VerticalFOV = verticalFOV;
        FocalLength = focalLength;
        Origin = origin;
    }
}
