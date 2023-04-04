using System;
using RayTracer.Imaging.Plugins;
using RayTracer.Library.CLI;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Render.Core;
using RayTracer.Render.Sample;

// --source=example.obj --output=output.png

CameraSettings settings = CameraSettings.Default with
{
    OriginTransform = WorldTransform.Identity.Translate(new(0, -1, 0)),
    DirectionTransform = WorldTransform.Identity.RotateX(MathHelper.DegToRad(-90))
};

try
{
    ArgSwitchesProcessor.Process(args);
    new PluginLoader().LoadPlugins();
    new RayCasterApp(settings).Run();
}
catch (Exception e)
{
    Log.Default.Error(e.Message);
}
