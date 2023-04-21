namespace RayTracer.Imaging.Png.PngChunks;

public readonly struct PngHeader
{
    public static int Size => 13;
    public readonly uint Width;
    
    public readonly uint Height;
    
    public readonly byte BitDepth;
    
    public readonly PngColorType PngColorType;
    
    public readonly byte CompressionMethod;
    
    public readonly byte FilterMethod;
    
    public readonly PngInterlaceMethod InterlaceMethod;
    
    public PngHeader(int width, int height)
    {
        Width = (uint)width;
        Height = (uint)height;
        BitDepth = 8;
        PngColorType = PngColorType.ColorUsed;
        CompressionMethod = 0;
        FilterMethod = 0;
        InterlaceMethod = PngInterlaceMethod.None;
    }

    public PngHeader(uint width, uint height, byte bitDepth, byte colorType, byte compressionMethod, byte filterMethod, byte interlaceMethod)
    {
        Width = width;
        Height = height;
        BitDepth = bitDepth;
        PngColorType = (PngColorType)colorType;
        CompressionMethod = compressionMethod;
        FilterMethod = filterMethod;
        InterlaceMethod = (PngInterlaceMethod)interlaceMethod;
    }
}