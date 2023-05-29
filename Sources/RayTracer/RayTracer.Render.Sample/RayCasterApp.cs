using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Immutable;
using RayTracer.DependencyInjection;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.CLI;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using RayTracer.Render.Core;
using RayTracer.Render.IO;
using RayTracer.Render.Sample.Configuration;
using RayTracer.Library.Utils;
using RayTracer.Render.Lights;
using RayTracer.Imaging.Plugins;

namespace RayTracer.Render.Sample;

public class RayCasterApp : IArgSwitchesProvider
{
    [Service]
    private readonly ArgSwitchesProcessor _switchesProcessor = null!;

    [Service]
    private readonly BitmapWritersIndexer _writersIndexer = null!;

    [Service]
    private readonly RayCasterConfiguration _configuration = null!;

    private readonly string[] _args;

    public CameraSettings CameraSettings { get; }

    IReadOnlyList<object> IArgSwitchesProvider.Listeners => new[] { _configuration };

    public RayCasterApp(string[] args, in CameraSettings settings)
    {
        CameraSettings = settings;
        _args = args;
    }

    public void Run()
    {
        _switchesProcessor.Process(_args, this);

        new PluginLoader().LoadPlugins();
        
        RayCasterSetup setup = new(_configuration);

        if (!_writersIndexer.Writers.TryGetValue(setup.TargetFormat, out var writer))
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
        BitmapRenderer renderer = new();

        renderer.Render(camera, scene);

        Bitmap result = renderer.GetResult();

        Log.Default.Info($"Writing to destination file {setup.Output}");

        using (var destinationFs = File.OpenWrite(setup.Output))
            writer.Write(destinationFs, result);
    }
}
