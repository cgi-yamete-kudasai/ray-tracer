using System;
using System.Diagnostics.CodeAnalysis;

namespace RayTracer.Render.Sample.Configuration;

public class RayCasterSetup
{
    private const string OBJ_EXTENSION = ".obj";

    public string Source { get; private set; }

    public string Output { get; private set; }

    public string TargetFormat { get; private set; }

    public RayCasterSetup(RayCasterConfiguration config)
    {
        ParseOutput(config);
        ParseSource(config);
    }

    [MemberNotNull(nameof(Source))]
    private void ParseSource(RayCasterConfiguration config)
    {
        Source = config.Source ?? throw new InvalidOperationException("Source must be provided");

        if (!Source.EndsWith(OBJ_EXTENSION))
            throw new InvalidOperationException($"The source file {Source} be an {OBJ_EXTENSION} file.");
    }

    [MemberNotNull(nameof(Output))]
    [MemberNotNull(nameof(TargetFormat))]
    private void ParseOutput(RayCasterConfiguration config)
    {
        Output = config.Output ?? throw new InvalidOperationException("Output must be provided");

        int dotIndex = config.Output.LastIndexOf('.');

        if (dotIndex == -1 || dotIndex == config.Output.Length - 1)
            throw new InvalidOperationException("Can't parse output file's format");

        TargetFormat = config.Output[(dotIndex + 1)..];
    }
}
