using System;
using System.IO;
using System.IO.Compression;
using RayTracer.Imaging;
using RayTracer.Imaging.IO.Readers;
using RayTracer.Imaging.Png;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Memory;
using RayTracer.Library.Utils;

namespace RayTracer.Png.Reader;

public class PngBitmapReader : IBitmapReader
{
    public ReadOnlySpan<byte> MagicBytes => FileSignatures.Png;

    public Bitmap Read(Stream source)
    {
        byte[] magicBytes = new byte[MagicBytes.Length];
        int bytesRead = source.Read(magicBytes);

        if (bytesRead != MagicBytes.Length || !MagicBytes.SequenceEqual(magicBytes))
            throw new ArgumentException("The file signature doesn't match the PNG signature.", nameof(source));

        PngHeader header = ReadIHDRChunk(source);

        Bitmap image = new((int)(uint)header.Width, (int)(uint)header.Height);

        ReadIDATChunk(source, image);
        ReadIENDChunk(source);

        return image;
    }

    private static unsafe PngHeader ReadIHDRChunk(Stream source)
    {
        PngChunk ihdrChunk = ReadChunk(source);
        Assert.True(ihdrChunk.Type.SequenceEqual(PngChunkTypes.IHDR));
        Assert.Equal(ihdrChunk.Data.Length, PngHeader.HeaderSize);

        PngHeader header;

        fixed (byte* ptr = ihdrChunk.Data)
            header = *(PngHeader*)ptr;

        return header;
    }

    private static void ReadIDATChunk(Stream source, Bitmap image)
    {
        PngChunk idatChunk = ReadChunk(source);
        Assert.True(idatChunk.Type.SequenceEqual(PngChunkTypes.IDAT));

        MemoryStream ms = new(idatChunk.Data.ToArray());
        MemoryStream decompressed = new();

        using (DeflateStream deflateStream = new(ms, CompressionMode.Decompress))
            deflateStream.CopyTo(decompressed);

        ReadOnlySpan<byte> data = decompressed.GetBuffer().AsSpan()[..(int)decompressed.Length];

        for (int i = 0; i < image.Height; i++)
        {
            Assert.Equal(0, data[i * image.Height]);
            
            for (int j = 0; j < image.Width; j++)
            {
                byte r = data[i * (image.Width * 3 + 1) + j * 3 + 1];
                byte g = data[i * (image.Width * 3 + 1) + j * 3 + 2];
                byte b = data[i * (image.Width * 3 + 1) + j * 3 + 3];

                ColorRGB color = new(r / 255f, g / 255f, b / 255f);
                image.SetColor(j, i, color);
            }
        }
    }

    private static void ReadIENDChunk(Stream source)
    {
        PngChunk iendChunk = ReadChunk(source);
        Assert.True(iendChunk.Type.SequenceEqual(PngChunkTypes.IEND));
        Assert.Equal(0, iendChunk.Data.Length);
    }

    private static PngChunk ReadChunk(Stream source)
    {
        uint length = source.NativeRead<BigEndianInt>();

        Span<byte> chunkType = new byte[4];
        
        Assert.Equal(4, source.Read(chunkType));

        Span<byte> chunkData = new byte[length];

        Assert.Equal((int)length, source.Read(chunkData));

        source.ReadByte();
        source.ReadByte();
        source.ReadByte();
        source.ReadByte();

        return new(chunkType, chunkData);
    }
}
