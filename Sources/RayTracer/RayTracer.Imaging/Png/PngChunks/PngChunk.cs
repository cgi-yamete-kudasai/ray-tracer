using System;

namespace RayTracer.Imaging.Png.PngChunks;

public readonly ref struct PngChunk
{
    public readonly uint DataLength;
    public readonly ReadOnlySpan<byte> ChunkType;
    public readonly ReadOnlySpan<byte> Data;
    public readonly uint Crc;

    public PngChunk(uint dataLength, Span<byte> chunkType, byte[] data) : this(dataLength, chunkType, data, Crc32.Calculate(data))
    {
    }
    
    public PngChunk(uint dataLength, Span<byte> chunkType, byte[] data, uint crc)
    {
        DataLength = dataLength;
        ChunkType = chunkType;
        Data = data;
        Crc = Crc32.Calculate(data);
    }
}