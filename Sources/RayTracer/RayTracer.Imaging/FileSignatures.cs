using System;

namespace RayTracer.Imaging;

public static class FileSignatures
{
    public static ReadOnlySpan<byte> Bmp => "BM"u8;

    public static ReadOnlySpan<byte> Ppm => "P3\n"u8;

    public static ReadOnlySpan<byte> Png => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

}
