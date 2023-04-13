using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RayTracer.Imaging.Bmp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct BmpHeader
{
    public static readonly int HeaderSize = Unsafe.SizeOf<BmpHeader>();

    // Header
    public readonly uint FileSize;

    public readonly uint Reserved;

    public readonly uint DataOffset;

    // InfoHeader
    public readonly uint Size = 40;

    public readonly uint Width;

    public readonly uint Height;

    public readonly ushort Planes = 1;

    public readonly ushort BitsPerPixel;

    public readonly uint Compression;

    public readonly uint ImageSize;

    public readonly uint XPixelsPerM;

    public readonly uint YPixelsPerM;

    public readonly uint ColorsUsed;

    public readonly uint ImportantColors;

    public BmpHeader(uint width, uint height)
    {
        Width = width;
        Height = height;
        BitsPerPixel = 24;
        DataOffset = (uint)HeaderSize;
        FileSize = (uint)HeaderSize + BitsPerPixel * Width * Height / 32 * 4;
    }
}
