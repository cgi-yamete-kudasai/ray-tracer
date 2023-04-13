using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RayTracer.Imaging.Bmp;

[StructLayout(LayoutKind.Explicit)]
public readonly struct BmpHeader
{
    public static readonly int HeaderSize = Unsafe.SizeOf<BmpHeader>();

    // Header
    [FieldOffset(0)]
    public readonly uint FileSize;

    [FieldOffset(4)]
    public readonly uint Reserved;

    [FieldOffset(8)]
    public readonly uint DataOffset;

    // InfoHeader
    [FieldOffset(12)]
    public readonly uint Size = 40;

    [FieldOffset(16)]
    public readonly uint Width;

    [FieldOffset(20)]
    public readonly uint Height;

    [FieldOffset(24)]
    public readonly ushort Planes = 1;

    [FieldOffset(26)]
    public readonly ushort BitsPerPixel;

    [FieldOffset(28)]
    public readonly uint Compression;

    [FieldOffset(32)]
    public readonly uint ImageSize;

    [FieldOffset(36)]
    public readonly uint XPixelsPerM;

    [FieldOffset(40)]
    public readonly uint YPixelsPerM;

    [FieldOffset(44)]
    public readonly uint ColorsUsed;

    [FieldOffset(48)]
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
