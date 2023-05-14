using RayTracer.Library.CLI;

namespace RayTracer.Imaging.Sample.Configuration;

public class ImageConverterConfiguration
{
    public const string SOURCE_ARG = "--source";

    public const string TARGET_ARG = "--goal-format";

    public const string OUTPUT_ARG = "--output";

    public string? Source { get; [ArgSwitch(SOURCE_ARG)] private set; }

    public string? Target { get; [ArgSwitch(TARGET_ARG)] private set; }

    public string? Output { get; [ArgSwitch(OUTPUT_ARG)] private set; }
}
