using System;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Shapes;
using System.Threading.Tasks;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;
using RayTracer.Render.Scenes;
using System.Drawing;

namespace RayTracer.Render.Core;

public class BitmapRenderer : IRenderer
{
    private const float ACNE_TOLERANCE = 0.001f;

    private Bitmap? _bitmap;

    public void Render(Camera camera, Scene scene)
    {
        int imageWidth = camera.ImageWidth;
        int imageHeight = camera.ImageHeight;

        Bitmap map = new(camera.ImageWidth, camera.ImageHeight);

        IntersectableList list = new();

        foreach (var descriptor in scene.Shapes)
        {
            var shape = descriptor.Shape;
            shape.Transform(descriptor.Transform);
            list.Add(shape);
        }

        Parallel.For(0, imageHeight, i =>
        {
            for (int j = 0; j < imageWidth; j++)
            {
                Ray ray = camera.GetRay(j, i);

                if (list.TryIntersect(ray, out var result))
                {
                    ColorRGB color = PaintPoint(result);
                    map.SetColor(j, i, color);
                }
            }

            ColorRGB PaintPoint(in IntersectionResult result)
            {
                int lightsCount = scene.Lights.Length;

                float r = 0;
                float g = 0;
                float b = 0;

                for (int k = 0; k < lightsCount; k++)
                {
                    var light = scene.Lights[k];

                    if (light.TryGetDirection(result, out var lightDir))
                    {
                        Ray lightRay = new(result.Point - ACNE_TOLERANCE * lightDir, -1 * lightDir);

                        if (list.TryIntersectAny(lightRay, out _))
                            continue;
                    }

                    ColorRGB current = light.PaintPoint(list, result);
                    r += current.R;
                    g += current.G;
                    b += current.B;
                }

                return new(r / lightsCount, g / lightsCount, b / lightsCount);
            }
        });

        _bitmap = map;
    }

    public Bitmap GetResult() => _bitmap ?? throw new InvalidOperationException();
}
