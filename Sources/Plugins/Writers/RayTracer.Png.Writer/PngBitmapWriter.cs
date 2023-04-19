using System;
using System.IO;
using System.IO.Compression;
using RayTracer.Imaging;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Imaging.Png;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Memory;
using RayTracer.Library.Utils;

namespace RayTracer.Png.Writer;

public class PngBitmapWriter : IBitmapWriter
{
    public string Format => "png";

    public void Write(Stream destination, Bitmap bitmap)
    {
        destination.Write(FileSignatures.Png);
        WriteIHDRChunk(destination, bitmap);
        WriteIDATChunk(destination, bitmap);
        WriteIENDChunk(destination);
    }

    private static unsafe void WriteIHDRChunk(Stream destination, Bitmap bitmap)
    {
        PngHeader header = new((uint)bitmap.Width, (uint)bitmap.Height, 8, 2);
        PngChunk chunk = new(PngChunkTypes.IHDR, new(&header, PngHeader.HeaderSize));
        WriteChunk(destination, chunk);
    }

    private static void WriteIDATChunk(Stream destination, Bitmap bitmap)
    {
        MemoryStream ms = new();

        for (int i = 0; i < bitmap.Height; i++)
        {
            ms.WriteByte(0);

            for (int j = 0; j < bitmap.Width; j++)
            {
                ColorRGB color = bitmap.Get(j, i);
                ms.WriteByte((byte)(color.R * 255));
                ms.WriteByte((byte)(color.G * 255));
                ms.WriteByte((byte)(color.B * 255));
            }
        }

        ms.Seek(0, SeekOrigin.Begin);

        MemoryStream compressed = new();

        using var zipStream = new DeflateStream(compressed, CompressionMode.Compress);
        zipStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
        zipStream.Flush();

        PngChunk chunk = new(PngChunkTypes.IDAT, compressed.GetBuffer().AsSpan()[..(int)compressed.Length]);

        WriteChunk(destination, chunk);
    }

    private static void WriteIENDChunk(Stream destination)
    {
        PngChunk chunk = new(PngChunkTypes.IEND);
        WriteChunk(destination, chunk);
    }

    private static void WriteChunk(Stream destination, PngChunk chunk)
    {
        int length = chunk.Data.Length;
        
        destination.NativeWrite(new BigEndianInt((uint)length));

        destination.Write(chunk.Type);

        destination.Write(chunk.Data);

        Crc32 crc = new();
        crc.Append(chunk.Type);
        crc.Append(chunk.Data);

        destination.NativeWrite(new BigEndianInt(crc.Get()));
    }
}
