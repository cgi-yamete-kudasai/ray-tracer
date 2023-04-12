﻿using System;
using System.IO;
using RayTracer.Imaging;
using RayTracer.Imaging.Bmp;
using RayTracer.Imaging.IO.Readers;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Bmp.Reader;

public class BmpBitmapReader : IBitmapReader
{
    public ReadOnlySpan<byte> MagicBytes => FileSignatures.Bmp;

    public Bitmap Read(Stream source)
    {
        byte[] magicBytes = new byte[MagicBytes.Length];
        int bytesRead = source.Read(magicBytes);

        if (bytesRead != MagicBytes.Length || !MagicBytes.SequenceEqual(magicBytes))
            throw new ArgumentException("The file signature doesn't match the BMP signature.", nameof(source));
        
        var header = source.MarshalReadStructure<BmpHeader>();
        ValidateHeader(header);

        Bitmap result = new((int)header.Width, (int)header.Height);

        source.Seek(header.DataOffset, SeekOrigin.Begin);

        int bytesToSkip = 4 - (int)header.Width * 3 % 4;

        if (bytesToSkip == 4)
            bytesToSkip = 0;

        for (int i = (int)header.Height - 1; i >= 0; i--)
        {
            for (int j = 0; j < header.Width; j++)
            {
                byte b = (byte)source.ReadByte();
                byte g = (byte)source.ReadByte();
                byte r = (byte)source.ReadByte();

                ColorRGB color = new(r / 255f, g / 255f, b / 255f);
                result.SetColor(j, i, color);
            }

            source.Seek(bytesToSkip, SeekOrigin.Current);
        }

        return result;
    }

    private static void ValidateHeader(in BmpHeader header)
    {
        Assert.Equal(0u, header.Reserved);

        Assert.Equal(1, header.Planes);

        Assert.Equal(24, header.BitsPerPixel);

        Assert.Equal(0u, header.Compression);
    }
}
