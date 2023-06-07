using System;
using System.Diagnostics;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Shapes;
using System.Threading.Tasks;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.IIntersectableTrees.OctTrees;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;
using RayTracer.Render.Scenes;

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

        OctTree tree = new();

        foreach (var descriptor in scene.Shapes)
        {
            var shape = descriptor.Shape;
            shape.Transform(descriptor.Transform);
            tree.Add(shape);
        }

        tree.UpdateTree();
        
        Stopwatch stopwatch = Stopwatch.StartNew();
        Log.Default.WriteLine("Starting render...", LogSeverity.Info);
        
        Parallel.For(0, imageHeight, i =>
        {
            for (int j = 0; j < imageWidth; j++)
            {
                Ray ray = camera.GetRay(j, i);

                if (tree.TryIntersect(ray, out var result))
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

                        if (tree.TryIntersectAny(lightRay, out _))
                            continue;
                    }

                    ColorRGB current = light.PaintPoint(tree, result);
                    r += current.R;
                    g += current.G;
                    b += current.B;
                }

                return new(r / lightsCount, g / lightsCount, b / lightsCount);
            }
        });

        stopwatch.Stop();
        Log.Default.WriteLine($"Render completed in {stopwatch.ElapsedMilliseconds}ms", LogSeverity.Info);

        _bitmap = map;
    }

    public Bitmap GetResult() => _bitmap ?? throw new InvalidOperationException();
}
