using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Utils;

public readonly struct CameraSettings
{
    public static readonly CameraSettings Default = new(16f / 9f, 1920, 2, 1, Vector3.Zero);

    public readonly float AspectRatio;

    public readonly int ImageHeight;

    public readonly float ViewportHeight;

    public readonly float FocalLength;

    public readonly Vector3 Origin;

    public CameraSettings(
        float aspectRatio,
        int imageHeight,
        float viewportHeight,
        float focalLength,
        in Vector3 origin)
    {
        AspectRatio = aspectRatio;
        ImageHeight = imageHeight;
        ViewportHeight = viewportHeight;
        FocalLength = focalLength;
        Origin = origin;
    }
}
