using System;
using System.Diagnostics.CodeAnalysis;

namespace RayTracer.Imaging.Sample.Configuration;

public class ImageConverterSetup
{
    public string Source { get; private set; }

    public string Output { get; private set; }

    public string TargetFormat { get; private set; }

    public ImageConverterSetup(ImageConverterConfiguration config)
    {
        ParseTarget(config);
        ParseOutput(config);
        ParseSource(config);
    }

    [MemberNotNull(nameof(Source))]
    [MemberNotNull(nameof(Output))]
    private void ParseSource(ImageConverterConfiguration config)
    {
        Source = config.Source ?? throw new InvalidOperationException("Source must be provided");

        int dotIndex = config.Source.LastIndexOf('.');

        Output ??= $"{Source[..dotIndex]}.{TargetFormat.ToLower()}";
    }

    [MemberNotNull(nameof(TargetFormat))]
    private void ParseTarget(ImageConverterConfiguration config)
    {
        TargetFormat = config.Target ?? throw new InvalidOperationException("Target must be provided");
    }

    private void ParseOutput(ImageConverterConfiguration config)
    {
        if (config.Output is null)
            return;

        int dotIndex = config.Output.LastIndexOf('.');

        if (dotIndex == -1 || dotIndex == config.Output.Length - 1)
            throw new InvalidOperationException("Can't parse output file's format");

        string format = config.Output[(dotIndex + 1)..];

        if (format != TargetFormat)
            throw new InvalidOperationException("Output file's format doesn't match the target format");

        Output = config.Output;
    }
}
