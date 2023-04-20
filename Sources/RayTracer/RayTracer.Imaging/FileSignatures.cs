using System;

namespace RayTracer.Imaging;

public static class FileSignatures
{
    public static ReadOnlySpan<byte> Bmp => "BM"u8;

    public static ReadOnlySpan<byte> Ppm => "P3\n"u8;
    public static ReadOnlySpan<byte> Png => new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
}
