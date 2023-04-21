using System;
using System.IO;
using System.IO.Compression;
using RayTracer.Imaging.Png.Filters;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.Png;

public static class PngEncoder
{
    public static void EncodeImageData(Bitmap bitmap, Stream outputStream)
    {
        MemoryStream dataStream = new MemoryStream();
        Span<byte> previousRaw = stackalloc byte[bitmap.Width * 3];
        Span<byte> currentRaw = stackalloc byte[bitmap.Width * 3];
        previousRaw.Fill(0);

        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                ColorRGB color = bitmap.Get(j, i);

                byte r = (byte)(color.R * 255);
                byte g = (byte)(color.G * 255);
                byte b = (byte)(color.B * 255);

                currentRaw[j * 3] = r;
                currentRaw[j * 3 + 1] = g;
                currentRaw[j * 3 + 2] = b;
            }

            FilterType filter = PngFilterProcessor.FindBestFilter(bitmap, previousRaw, currentRaw);
            dataStream.WriteByte((byte)filter);
            PngFilterProcessor.Process(previousRaw, currentRaw, filter, dataStream, FilterMode.Apply);
            currentRaw.CopyTo(previousRaw);
        }

        using var deflateStream = new DeflateStream(outputStream, CompressionMode.Compress);

        deflateStream.Write(dataStream.ToArray());
        deflateStream.Flush();
    }
}