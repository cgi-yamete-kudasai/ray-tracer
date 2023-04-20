using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RayTracer.Imaging.Png;

public readonly struct PngHeader
{
    public readonly uint Width;
    
    public readonly uint Height;
    
    public readonly byte BitDepth;
    
    public readonly PngColorType PngColorType;
    
    public readonly byte CompressionMethod;
    
    public readonly byte FilterMethod;
    
    public readonly byte InterlaceMethod;
    
    public PngHeader(uint width, uint height)
    {
        Width = width;
        Height = height;
        BitDepth = 8;
        PngColorType = (PngColorType)2;
        CompressionMethod = 0;
        FilterMethod = 0;
        InterlaceMethod = 0;
    }

    public PngHeader(uint width, uint height, byte bitDepth, byte colorType, byte compressionMethod, byte filterMethod, byte interlaceMethod)
    {
        Width = width;
        Height = height;
        BitDepth = bitDepth;
        PngColorType = (PngColorType)colorType;
        CompressionMethod = compressionMethod;
        FilterMethod = filterMethod;
        InterlaceMethod = interlaceMethod;
    }
}