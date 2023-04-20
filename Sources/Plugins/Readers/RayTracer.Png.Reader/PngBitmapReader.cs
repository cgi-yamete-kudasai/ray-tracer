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
        Span<byte> header = new byte[8];

        Assert.Equal(8, source.Read(header));
        Assert.True(header.SequenceEqual(MagicBytes));

        var firstChunk = ReadPngChunk(source);
        var pngHeader = ReadIHDRChunk(firstChunk);
        var dataStream = new MemoryStream();
        while (source.Position < source.Length)
        {
            PngChunk pngChunk = ReadPngChunk(source);

            if (pngChunk.ChunkType.SequenceEqual(PngChunkType.IDAT))
            {
                ReadIDATChunk(pngChunk, dataStream);
            }
            else if (pngChunk.ChunkType.SequenceEqual(PngChunkType.IEND))
            {
                break;
            }
        }

        Assert.Equal(source.Length, source.Position);

        var bitmap = DecodeImageData(dataStream, pngHeader);

        return bitmap;
    }

    private static Bitmap DecodeImageData(Stream dataStream, PngHeader pngHeader)
    {
        dataStream.Flush();
        dataStream.Seek(2, SeekOrigin.Begin);

        MemoryStream output = new MemoryStream();

        using (var deflateStream = new DeflateStream(dataStream, CompressionMode.Decompress))
        {
            deflateStream.CopyTo(output);
            deflateStream.Close();
        }

        PngDecoder.Decode(output, pngHeader);

        Bitmap bitmap = new Bitmap((int)pngHeader.Width, (int)pngHeader.Height);

        switch (pngHeader.PngColorType)
        {
            case PngColorType.None:
                ReadGrayScaleImage(output, bitmap);
                break;
            case PngColorType.ColorUsed:
                ReadRGBImage(output, bitmap);
                break;
            case PngColorType.PaletteUsed:
            case PngColorType.AlphaChannelUsed:
            case PngColorType.ColorUsed | PngColorType.AlphaChannelUsed:
                throw new NotImplementedException();
            default:
                throw new ArgumentException();
        }


        return bitmap;
    }

    private static void ReadRGBImage(Stream stream, Bitmap bitmap)
    {
        for (int i = 0; i < bitmap.Height; i++)
        {
            stream.ReadByte();
            for (int j = 0; j < bitmap.Width; j++)
            {
                var r = stream.ReadByte();
                var g = stream.ReadByte();
                var b = stream.ReadByte();
                bitmap.SetColor(j, i, new ColorRGB(r / 255f, g / 255f, b / 255f));
            }
        }
    }

    private static void ReadGrayScaleImage(Stream memoryStream, Bitmap bitmap)
    {
        for (int i = 0; i < bitmap.Height; i++)
        {
            memoryStream.ReadByte();
            for (int j = 0; j < bitmap.Width; j++)
            {
                var value = memoryStream.ReadByte();
                bitmap.SetColor(j, i, new ColorRGB(value / 255f, value / 255f, value / 255f));
            }
        }
    }

    private static PngHeader ReadIHDRChunk(PngChunk pngChunk)
    {
        Assert.True(pngChunk.ChunkType.SequenceEqual(PngChunkType.IHDR));
        Assert.Equal((uint)13, pngChunk.DataLength);
        Assert.Equal(13, pngChunk.Data.Length);

        MemoryStream dataStream = new MemoryStream(pngChunk.Data);

        uint width = dataStream.NativeRead<BigEndianInt>();
        uint height = dataStream.NativeRead<BigEndianInt>();
        byte bitDepth = dataStream.NativeRead<byte>();
        byte colorType = dataStream.NativeRead<byte>();
        byte compressionMethod = dataStream.NativeRead<byte>();
        byte filterMethod = dataStream.NativeRead<byte>();
        byte interlaceMethod = dataStream.NativeRead<byte>();

        return new PngHeader(width, height, bitDepth, colorType, compressionMethod, filterMethod, interlaceMethod);
    }

    private static void ReadIDATChunk(PngChunk pngChunk, MemoryStream dataStream)
    {
        Assert.True(pngChunk.ChunkType.SequenceEqual(PngChunkType.IDAT));
        dataStream.Write(pngChunk.Data, 0, pngChunk.Data.Length);
    }

    private static PngChunk ReadPngChunk(Stream stream)
    {
        uint length = stream.NativeRead<BigEndianInt>();

        var chunkType = ReadChunkType(stream);

        byte[] chunkData = new byte[length];
        Assert.Equal(length, (uint)stream.Read(chunkData));

        uint crc = stream.NativeRead<BigEndianInt>();

        return new PngChunk(length, chunkType, chunkData, crc);
    }

    private static Span<byte> ReadChunkType(Stream stream)
    {
        Span<byte> chunkType = new byte[4];
        Assert.Equal(4, stream.Read(chunkType));
        return chunkType;
    }
}