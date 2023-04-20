using System;

namespace RayTracer.Imaging.Png;

public static class PngChunkType
{
    public static ReadOnlySpan<byte> IEND => "IEND"u8;
    public static ReadOnlySpan<byte> IDAT => "IDAT"u8;
    public static ReadOnlySpan<byte> IHDR => "IHDR"u8;
}