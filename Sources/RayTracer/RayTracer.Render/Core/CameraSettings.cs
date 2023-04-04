using RayTracer.Library.Mathematics;

namespace RayTracer.Render.Core;

public readonly record struct CameraSettings(
    float AspectRatio,
    int ImageHeight,
    float VerticalFOV,
    float FocalLength,
    WorldTransform OriginTransform,
    WorldTransform DirectionTransform)
{
    public static readonly CameraSettings Default = new()
    {
        AspectRatio = 16f / 9,
        ImageHeight = 1080,
        VerticalFOV = MathHelper.DegToRad(90),
        FocalLength = 1,
        OriginTransform = WorldTransform.Identity,
        DirectionTransform = WorldTransform.Identity
    };
}
