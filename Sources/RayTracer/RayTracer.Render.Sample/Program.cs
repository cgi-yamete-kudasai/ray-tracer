using RayTracer.Imaging.Plugins;
using RayTracer.Library.CLI;
using RayTracer.Render.Core;
using RayTracer.Render.Sample;

// --source=example.obj --output=output.png

CameraSettings settings = CameraSettings.Default;

ArgSwitchesProcessor.Process(args);
new PluginLoader().LoadPlugins();
new RayCasterApp(settings).Run();
