using System.IO;
using System;
using System.Collections.Immutable;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using RayTracer.Render.Core;
using RayTracer.Render.IO;
using RayTracer.Render.Sample.Configuration;
using RayTracer.Library.Utils;
using RayTracer.Render.Lights;

namespace RayTracer.Render.Sample;

public class RayCasterApp
{
    public CameraSettings CameraSettings { get; }

    public RayCasterApp(in CameraSettings settings)
    {
        CameraSettings = settings;
    }

    public void Run()
    {
        RayCasterSetup setup = new(RayCasterConfiguration.Instance);

        if (!BitmapWritersIndexer.Instance.Writers.TryGetValue(setup.TargetFormat, out var writer))
            throw new InvalidOperationException($"Format {setup.TargetFormat} is not supported for writing");

        if (!File.Exists(setup.Source))
            throw new FileNotFoundException($"Can't find source file {setup.Source}.");

        Scene scene;

        Log.Default.Info($"Loading source file {setup.Source}");

        using (var sourceFs = File.OpenRead(setup.Source))
        {
            // hardcoded lights
            var lights = ImmutableArray<ILight>.Empty.Add(new DirectionalLight(new(0, 1, 0)));
            var shapes = new ObjMeshReader().Read(sourceFs);

            scene = new(shapes, lights);
        }

        Log.Default.Info($"Rendering with settings: {CameraSettings}");

        Camera camera = new(CameraSettings);
        Bitmap result = camera.Render(scene);

        Log.Default.Info($"Writing to destination file {setup.Output}");

        using (var destinationFs = File.OpenWrite(setup.Output))
            writer.Write(destinationFs, result);
    }
}
