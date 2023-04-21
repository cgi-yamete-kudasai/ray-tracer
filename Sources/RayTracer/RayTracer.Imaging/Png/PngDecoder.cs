using System;
using System.IO;
using System.IO.Compression;
using RayTracer.Imaging.Png.Filters;
using RayTracer.Imaging.Png.PngChunks;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.Png;

public static class PngDecoder
{
    public static Bitmap DecodeImageData(Stream dataStream, PngHeader pngHeader)
    {
        dataStream.Flush();
        dataStream.Seek(2, SeekOrigin.Begin);
        
        MemoryStream output = new MemoryStream();

        using (var deflateStream = new DeflateStream(dataStream, CompressionMode.Decompress))
        {
            deflateStream.CopyTo(output);
            deflateStream.Close();
        }

        DecodeDecompressedData(output, pngHeader);

        Bitmap bitmap = new Bitmap((int)pngHeader.Width, (int)pngHeader.Height);

        switch (pngHeader.PngColorType)
        {
            case PngColorType.GreyScale:
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

    private static void DecodeDecompressedData(MemoryStream decompressedData, PngHeader pngHeader)
    {
        if (pngHeader.InterlaceMethod == PngInterlaceMethod.Adam7)
        {
            throw new NotImplementedException("Interlaced PNGs are not supported.");
        }
        
        var bytesPerScanline = GetBytesPerScanline(pngHeader);

        var data = decompressedData.GetBuffer();

        Span<byte> currentRow;
        Span<byte> previousRow = new byte[bytesPerScanline];
        previousRow.Fill(0);
        MemoryStream output = new MemoryStream();
        
        for (int i = 0; i < pngHeader.Height; i++)
        {
            var filterType = (FilterType)data[i * (bytesPerScanline + 1)];
            currentRow = data.AsSpan(i * (bytesPerScanline + 1) + 1, bytesPerScanline);
            PngFilterProcessor.Process(previousRow,currentRow,filterType, output, FilterMode.Reverse,GetBytesPerPixel(pngHeader));
            currentRow.CopyTo(previousRow);
        }
        
        decompressedData.SetLength(output.Length);
        output.Position = 0;
        output.CopyTo(decompressedData);
        decompressedData.Position = 0;
    }

    private static byte GetSamplesPerPixel(PngHeader header)
    {
        switch (header.PngColorType)
        {
            case PngColorType.GreyScale:
                return 1;
            case PngColorType.PaletteUsed:
                return 1;
            case PngColorType.ColorUsed:
                return 3;
            case PngColorType.AlphaChannelUsed:
                return 1;
            case PngColorType.ColorUsed | PngColorType.AlphaChannelUsed:
                return 4;
            default:
                return 0;
        }
    }

    private static byte GetBytesPerPixel(PngHeader header)
    {
        var bitDepthCorrected = (header.BitDepth + 7) / 8;
        var samplesPerPixel = GetSamplesPerPixel(header);

        return (byte)(bitDepthCorrected * samplesPerPixel);
    }

    private static int GetBytesPerScanline(PngHeader header)
    {
        var width = header.Width;

        switch (header.BitDepth)
        {
            case 1:
                return (int)((width + 7) / 8);
            case 2:
                return (int)((width + 3) / 4);
            case 4:
                return (int)((width + 1) / 2);
            case 8:
            case 16:
                return (int)(width * GetSamplesPerPixel(header) * (header.BitDepth / 8));
            default:
                return 0;
        }
    }
}