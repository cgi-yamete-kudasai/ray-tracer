﻿using System.Runtime.CompilerServices;
using RayTracer.Library.Lights;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Utils;

public sealed class Camera
{
    public CameraSettings Settings { get; }

    public Camera(in CameraSettings settings)
    {
        Settings = settings;
    }

    public Bitmap Render(Scene scene)
    {
        DirectionalLight light = new(Vector3.Normalize(new(0, 0, -1)));

        int imageHeight = Settings.ImageHeight;
        int imageWidth = (int)(Settings.AspectRatio * imageHeight);

        float viewportHeight = Settings.ViewportHeight;
        float viewportWidth = Settings.AspectRatio * Settings.ViewportHeight;

        Vector3 horizontal = new(viewportWidth, 0, 0);
        Vector3 vertical = new(0, -viewportHeight, 0);
        Vector3 topLeftCorner = new(-viewportWidth / 2, viewportHeight / 2, -Settings.FocalLength);

        Bitmap map = new(imageWidth, imageHeight);

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                float u = (float)i / imageHeight;
                float v = (float)j / imageWidth;

                Vector3 direction = Settings.Origin + topLeftCorner + u * vertical + v * horizontal - Settings.Origin;
                Ray ray = new(Settings.Origin, direction);

                foreach (var shape in scene.Shapes)
                {
                    if (shape.TryIntersect(ray, out var point))
                    {
                        ColorRGB color = light.PaintPoint(shape, point);
                        map.SetColor(j, i, color);
                        break;
                    }
                }
            }
        }

        return map;
    }
}
