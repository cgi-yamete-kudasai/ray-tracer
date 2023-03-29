using RayTracer.Imaging.Plugins;
using RayTracer.Library.CLI;
using RayTracer.Render.Core;
using RayTracer.Render.Sample;

// --source=example.obj --output=output.png

CameraSettings settings = CameraSettings.Default with
{
    Origin = new(0, 0, 1)
};

ArgSwitchesProcessor.Process(args);
new PluginLoader().LoadPlugins();
new RayCasterApp(settings).Run();
