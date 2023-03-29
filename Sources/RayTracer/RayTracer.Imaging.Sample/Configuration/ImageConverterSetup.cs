using System;
using System.Diagnostics.CodeAnalysis;

namespace RayTracer.Imaging.Sample.Configuration;

public class ImageConverterSetup
{
    public string Source { get; private set; }

    public string Output { get; private set; }

    public ImageFormat SourceFormat { get; private set; }

    public ImageFormat TargetFormat { get; private set; }

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

        if (dotIndex == -1 || dotIndex == config.Source.Length - 1 || !TryParseFormat(config.Source.AsSpan()[(dotIndex + 1)..], out var format))
            throw new InvalidOperationException("Can't parse source format");

        Output ??= $"{Source[..dotIndex]}.{TargetFormat.ToString().ToLower()}";
        SourceFormat = format;
    }

    private void ParseTarget(ImageConverterConfiguration config)
    {
        if (config.Target is null)
            throw new InvalidOperationException("Target must be provided");

        if (!TryParseFormat(config.Target, out var format))
            throw new InvalidOperationException("Can't parse target format");

        TargetFormat = format;
    }

    private void ParseOutput(ImageConverterConfiguration config)
    {
        if (config.Output is null)
            return;

        int dotIndex = config.Output.LastIndexOf('.');

        if (dotIndex == -1 || dotIndex == config.Output.Length - 1 || !TryParseFormat(config.Output.AsSpan()[(dotIndex + 1)..], out var format))
            throw new InvalidOperationException("Can't parse output file's format");

        if (format != TargetFormat)
            throw new InvalidOperationException("Output file's format doesn't match the target format");

        Output = config.Output;
    }

    private static bool TryParseFormat(ReadOnlySpan<char> span, out ImageFormat format)
    {
        return Enum.TryParse(span, true, out format);
    }
}
