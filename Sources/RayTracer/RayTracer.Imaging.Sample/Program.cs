using System;
using RayTracer.Imaging.Sample;
using RayTracer.Library.CLI;
using RayTracer.Library.Diagnostics.Logging;
using RayTracer.Library.Extensions;
using RayTracer.DependencyInjection;
using RayTracer.Imaging.IO.Readers;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Imaging.Sample.Configuration;

// --source=example.jpg --goal-format=png [--output=output.png]

try
{
    var builder = ServiceContainer.CreateBuilder()
        .AddSingleton<ArgSwitchesProcessor>()
        .AddSingleton<ArgSwitchesIndexer>()
        .AddSingleton<BitmapWritersIndexer>()
        .AddSingleton<BitmapReadersIndexer>()
        .AddSingleton<ImageConverterConfiguration>()
        .AddSingleton<ConverterApp>(() => new(args));

    using var container = builder.Build();

    container.ResolveService<ConverterApp>().Run();
}
catch (Exception e)
{
    Log.Default.Error(e.Message);
}
