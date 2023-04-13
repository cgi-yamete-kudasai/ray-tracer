using System;

namespace RayTracer.Imaging.Png;

public readonly ref struct PngChunk
{
    public readonly ReadOnlySpan<byte> Type;

    public readonly ReadOnlySpan<byte> Data;

    public PngChunk(ReadOnlySpan<byte> type, ReadOnlySpan<byte> data = default)
    {
        Type = type;
        Data = data;
    }
}
