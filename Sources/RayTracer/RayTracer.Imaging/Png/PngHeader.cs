using RayTracer.Library.Memory;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RayTracer.Imaging.Png;

[StructLayout(LayoutKind.Sequential, Size = 13)]
public readonly struct PngHeader
{
    public static readonly int HeaderSize = Unsafe.SizeOf<PngHeader>();

    public readonly BigEndianInt Width;

    public readonly BigEndianInt Height;

    public readonly byte BitDepth;

    public readonly byte ColorType;

    public readonly byte CompressionMethod;

    public readonly byte FilterMethod;

    public readonly byte InterlaceMethod;

    public PngHeader(
        BigEndianInt width,
        BigEndianInt height,
        byte bitDepth,
        byte colorType)
    {
        Width = width;
        Height = height;
        BitDepth = bitDepth;
        ColorType = colorType;
    }
}
