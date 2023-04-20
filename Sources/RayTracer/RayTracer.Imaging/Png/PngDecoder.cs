using System;
using System.IO;

namespace RayTracer.Imaging.Png;

public static class PngDecoder
{
    public static void Decode(MemoryStream dataStream, PngHeader pngHeader)
    {
        if (pngHeader.InterlaceMethod == 1) // Adam7
        {
            throw new NotImplementedException("Interlaced PNGs are not supported.");
        }
        var bytesPerScanline = GetBytesPerScanline(pngHeader);

        var data = dataStream.GetBuffer();

        for (int i = 0; i < pngHeader.Height; i++)
        {
            var filterType = (FilterType)data[i * (bytesPerScanline + 1)];
            ReverseFilter(filterType, data, i, bytesPerScanline);
        }
        
        dataStream.Position = 0;
    }

    private static byte GetSamplesPerPixel(PngHeader header)
    {
        switch (header.PngColorType)
        {
            case PngColorType.None:
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

    private static void ApplyFilter(FilterType filterType, Span<byte> data, int rawId, int bytesPerScanline)
    {
        int rawStartIndex = rawId * (bytesPerScanline + 1) + 1;
        int previousRawStartIndex = (rawId - 1) * (bytesPerScanline + 1) + 1;
        
        
    }

    private static void ReverseFilter(FilterType filterType, Span<byte> decodedData, int rawId, int bytesPerScanline)
    {
        int rawStartIndex = rawId * (bytesPerScanline + 1) + 1;
        int previousRawStartIndex = (rawId - 1) * (bytesPerScanline + 1) + 1;
        
        for (int byteIndex = 0; byteIndex < bytesPerScanline; byteIndex++)
        {
            var byteValue = decodedData[rawStartIndex + byteIndex];
            var leftValue = GetLeftByte(byteIndex, decodedData);
            var aboveValue = GetAboveByte(byteIndex, decodedData);
            var aboveLeftValue = GetAboveLeftByte(byteIndex, decodedData);
            
            switch (filterType)
            {
                case FilterType.None:
                    break;
                case FilterType.Sub:
                    decodedData[rawStartIndex + byteIndex] = (byte)(byteValue + leftValue);
                    break;
                case FilterType.Up:
                    decodedData[rawStartIndex + byteIndex] = (byte)(byteValue + aboveValue);
                    break;
                case FilterType.Average:
                    decodedData[rawStartIndex + byteIndex] = (byte)(byteValue + (leftValue + aboveValue) / 2);
                    break;
                case FilterType.Paeth:
                    decodedData[rawStartIndex + byteIndex] = (byte)(byteValue + PaethPredictor(leftValue, aboveValue, aboveLeftValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }
        byte GetLeftByte(int byteIndex, Span<byte> data)
        {
            var leftIndex = byteIndex - 1;
            var leftValue = leftIndex >= 0 ? data[rawStartIndex + leftIndex] : (byte)0;
            return leftValue;
        }
        
        byte GetAboveByte(int byteIndex, Span<byte> data)
        {
            var upIndex = previousRawStartIndex + byteIndex;
            return upIndex >= 0 ? data[upIndex] : (byte)0;
        }
        
        byte GetAboveLeftByte(int byteIndex, Span<byte> data)
        {
            var index = previousRawStartIndex + byteIndex - 1;
            return index >= 0 ? data[index] : (byte)0;
        }
        
        byte PaethPredictor(byte a, byte b, byte c)
        {
            var p = a + b - c;
            var pa = Math.Abs(p - a);
            var pb = Math.Abs(p - b);
            var pc = Math.Abs(p - c);

            if (pa <= pb && pa <= pc)
            {
                return a;
            }

            if (pb <= pc)
            {
                return b;
            }

            return c;
        }
    }

    private enum FilterType
    {
        None = 0,
        Sub = 1,
        Up = 2,
        Average = 3,
        Paeth = 4
    }
}