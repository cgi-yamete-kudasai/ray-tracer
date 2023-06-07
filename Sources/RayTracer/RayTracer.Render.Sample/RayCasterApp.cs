using System;
using System.Collections.Generic;
using System.IO;
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
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Render.Scenes;

namespace RayTracer.Render.Sample;

public class RayCasterApp : IArgSwitchesProvider
{
    [Service]
    private readonly ArgSwitchesProcessor _switchesProcessor = null!;

    [Service]
    private readonly BitmapWritersIndexer _writersIndexer = null!;

    [Service]
    private readonly RayCasterConfiguration _configuration = null!;

    [Service]
    private readonly IMeshReader _meshReader = null!;

    [Service]
    private readonly ISceneLocator _sceneLocator = null!;

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

        Log.Default.Info($"Loading source file {setup.Source}");

        Scene scene = setup.SourceKind switch
        {
            SourceKind.Scene => ReadScene(setup.Source),
            SourceKind.Obj => ReadObjFile(setup.Source),
            _ => throw new NotSupportedException()
        };

        Log.Default.Info($"Rendering with settings: {CameraSettings}");

        Camera camera = new(CameraSettings);
        BitmapRenderer renderer = new();

        renderer.Render(camera, scene);

        Bitmap result = renderer.GetResult();

        Log.Default.Info($"Writing to destination file {setup.Output}");

        using var destinationFs = File.OpenWrite(setup.Output);
        writer.Write(destinationFs, result);
    }

    private Scene ReadScene(string sceneName)
    {
        return _sceneLocator.LocateScene(sceneName);
    }

    private Scene ReadObjFile(string sourceFile)
    {
        if (!File.Exists(sourceFile))
            throw new FileNotFoundException($"Can't find source file {sourceFile}.");

        var builder = CreateDefaultBuilder();
        builder.AddMeshFromFile(sourceFile, WorldTransform.Identity);
        return builder.Build();
    }

    private Scene.Builder CreateDefaultBuilder()
    {
        var builder = Scene.CreateBuilder(_meshReader);
        builder.AddLight(new PointLight(new(0, -1, 0)));
        return builder;
    }
}
