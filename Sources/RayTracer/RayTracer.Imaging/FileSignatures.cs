using System;

namespace RayTracer.Imaging;

public static class FileSignatures
{
    public static ReadOnlySpan<byte> Bmp => "BM"u8;

    public static ReadOnlySpan<byte> Ppm => "P3\n"u8;
}
