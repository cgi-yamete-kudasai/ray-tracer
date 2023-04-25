using RayTracer.Imaging.Plugins;
using RayTracer.Imaging.Sample;
using RayTracer.Library.CLI;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using System;

// --source=example.jpg --goal-format=png [--output=output.png]

try
{
    ArgSwitchesProcessor.Process(args);
    new PluginLoader().LoadPlugins();
    new ConverterApp().Run();
}
catch (Exception e)
{
    Log.Default.Error(e.Message);
}
