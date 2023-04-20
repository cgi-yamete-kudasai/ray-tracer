using System;

namespace RayTracer.Imaging.Png;

public ref struct PngChunk
{
    public uint DataLength;
    public ReadOnlySpan<byte> ChunkType;
    public byte[] Data;
    public uint Crc;

    public PngChunk(uint dataLength, Span<byte> chunkType, byte[] data, uint crc)
    {
        DataLength = dataLength;
        ChunkType = chunkType;
        Data = data;
        Crc = crc;
    }
}