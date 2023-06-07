using System;
using RayTracer.DependencyInjection;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.CLI;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Render.Core;
using RayTracer.Render.IO;
using RayTracer.Render.Sample;
using RayTracer.Render.Sample.Configuration;
using RayTracer.Render.Scenes;

// --source=example.obj --output=output.png

CameraSettings settings = CameraSettings.Default with
{
    OriginTransform = WorldTransform.Identity.Translate(new(0, -2f, 0)),
    DirectionTransform = WorldTransform.Identity.RotateX(MathHelper.DegToRad(-90)),
    ImageHeight = 300
};

try
{
    var builder = ServiceContainer.CreateBuilder()
        .AddSingleton<ArgSwitchesProcessor>()
        .AddSingleton<ArgSwitchesIndexer>()
        .AddSingleton<BitmapWritersIndexer>()
        .AddSingleton<RayCasterConfiguration>()
        .AddSingleton<ISceneLocator>(() => new InMemorySceneLocator())
        .AddSingleton<IMeshReader>(() => new ObjMeshReader())
        .AddSingleton<RayCasterApp>(() => new(args, settings));

    using var container = builder.Build();
    
    container.ResolveService<RayCasterApp>().Run();
}
catch (Exception e)
{
    Log.Default.Error(e.Message);
}
