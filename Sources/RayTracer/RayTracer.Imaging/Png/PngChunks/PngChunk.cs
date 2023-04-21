using System;

namespace RayTracer.Imaging.Png.PngChunks;

public ref struct PngChunk
{
    public uint DataLength;
    public ReadOnlySpan<byte> ChunkType;
    public byte[] Data;
    public uint Crc;

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