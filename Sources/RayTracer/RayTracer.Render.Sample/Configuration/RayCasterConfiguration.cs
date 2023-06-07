using RayTracer.Library.CLI;

namespace RayTracer.Render.Sample.Configuration;

public class RayCasterConfiguration
{
    public const string SOURCE_ARG = "--source";

    public const string OUTPUT_ARG = "--output";

    public string? Source { get; [ArgSwitch(SOURCE_ARG)] private set; }

    public string? Output { get; [ArgSwitch(OUTPUT_ARG)] private set; }
}
