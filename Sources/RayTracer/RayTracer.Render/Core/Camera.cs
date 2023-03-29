using System;
using System.Threading.Tasks;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;
using RayTracer.Library.Utils;

namespace RayTracer.Render.Core;

public sealed class Camera
{
    public CameraSettings Settings { get; }

    public Camera(in CameraSettings settings)
    {
        Settings = settings;
    }

    public Bitmap Render(Scene scene)
    {
        int imageHeight = Settings.ImageHeight;
        int imageWidth = (int)(Settings.AspectRatio * imageHeight);

        float viewportHeight = 2 * Settings.FocalLength * (float)Math.Tan(Settings.VerticalFOV / 2);
        float viewportWidth = Settings.AspectRatio * viewportHeight;

        Vector3 horizontal = new(viewportWidth, 0, 0);
        Vector3 vertical = new(0, -viewportHeight, 0);
        Vector3 topLeftCorner = new(-viewportWidth / 2, viewportHeight / 2, -Settings.FocalLength);

        Bitmap map = new(imageWidth, imageHeight);

        IntersectableList list = new(scene.Shapes);

        Parallel.For(0, imageHeight, i =>
        {
            for (int j = 0; j < imageWidth; j++)
            {
                float u = (float)i / imageHeight;
                float v = (float)j / imageWidth;

                Vector3 direction = Settings.Origin + topLeftCorner + u * vertical + v * horizontal - Settings.Origin;
                Ray ray = new(Settings.Origin, direction);

                if (list.TryIntersect(ray, out var result))
                {
                    // TODO: handle many lights
                    ColorRGB color = scene.Lights[0].PaintPoint(list, result);
                    map.SetColor(j, i, color);
                }
            }
        });

        return map;
    }
}
