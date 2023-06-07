using System;
using RayTracer.Library.Mathematics;

namespace RayTracer.Render.Core;

public sealed class Camera
{
    public CameraSettings Settings { get; }

    public int ImageHeight => Settings.ImageHeight;

    public int ImageWidth { get; }

    public Vector3 Origin { get; }

    private readonly Vector3 _horizontal;

    private readonly Vector3 _vertical;

    private readonly Vector3 _topLeftCorner;

    public Camera(in CameraSettings settings)
    {
        Settings = settings;

        ImageWidth = (int)(Settings.AspectRatio * ImageHeight);

        float viewportHeight = 2 * Settings.FocalLength * (float)Math.Tan(Settings.VerticalFOV / 2);
        float viewportWidth = Settings.AspectRatio * viewportHeight;

        Origin = Vector3.Zero.Transform(Settings.OriginTransform);

        Vector3 horizontal = new(viewportWidth, 0, 0);
        _horizontal = horizontal.Transform(Settings.DirectionTransform);

        Vector3 vertical = new(0, -viewportHeight, 0);
        _vertical = vertical.Transform(Settings.DirectionTransform);

        Vector3 topLeftCorner = new(-viewportWidth / 2, viewportHeight / 2, -Settings.FocalLength);
        _topLeftCorner = topLeftCorner.Transform(Settings.DirectionTransform);
    }

    public Ray GetRay(int x, int y)
    {
        float u = (float)y / ImageHeight;
        float v = (float)x / ImageWidth;

        Vector3 direction = _topLeftCorner + u * _vertical + v * _horizontal - Origin;
        return new(Origin, direction);
    }
}
