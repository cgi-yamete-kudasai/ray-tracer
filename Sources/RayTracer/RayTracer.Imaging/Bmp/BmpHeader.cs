using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RayTracer.Imaging.Bmp;

[StructLayout(LayoutKind.Explicit)]
public readonly struct BmpHeader
{
    public static readonly int HeaderSize = Unsafe.SizeOf<BmpHeader>();

    // Header
    [FieldOffset(0)]
    public readonly ushort Signature = 'M' << 8 | 'B';

    [FieldOffset(2)]
    public readonly uint FileSize;

    [FieldOffset(6)]
    public readonly uint Reserved;

    [FieldOffset(10)]
    public readonly uint DataOffset;

    // InfoHeader
    [FieldOffset(14)]
    public readonly uint Size = 40;

    [FieldOffset(18)]
    public readonly uint Width;

    [FieldOffset(22)]
    public readonly uint Height;

    [FieldOffset(26)]
    public readonly ushort Planes = 1;

    [FieldOffset(28)]
    public readonly ushort BitsPerPixel;

    [FieldOffset(30)]
    public readonly uint Compression;

    [FieldOffset(34)]
    public readonly uint ImageSize;

    [FieldOffset(38)]
    public readonly uint XPixelsPerM;

    [FieldOffset(42)]
    public readonly uint YPixelsPerM;

    [FieldOffset(46)]
    public readonly uint ColorsUsed;

    [FieldOffset(50)]
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
