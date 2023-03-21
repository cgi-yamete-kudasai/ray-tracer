using RayTracer.Library.CLI;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging;

public class ImageConverterConfiguration : Singleton<ImageConverterConfiguration>
{
    public const string SOURCE_ARG = "--source";

    public const string TARGET_ARG = "--goal-format";
    
    public const string OUTPUT_ARG = "--output";

    public string Source { get; [ArgSwitch(SOURCE_ARG)] private set; } = null!;

    public string Target { get; [ArgSwitch(TARGET_ARG)] private set; } = null!;

    public string Output { get; [ArgSwitch(OUTPUT_ARG)] private set; } = null!;
}
