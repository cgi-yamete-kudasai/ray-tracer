using System.IO;
using RayTracer.Library.Diagnostics;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Readers;

public class BmpBitmapReader : IBitmapReader
{
    public ImageFormat Format => ImageFormat.Bmp;

    public Bitmap Read(Stream source)
    {
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
        Assert.Equal('B', header.Signature & 0x00FF);
        Assert.Equal('M', (header.Signature & 0xFF00) >> 8);

        Assert.Equal(0u, header.Reserved);

        Assert.Equal(40u, header.Size);

        Assert.Equal(1, header.Planes);

        Assert.Equal(24, header.BitsPerPixel);

        Assert.Equal(0u, header.Compression);
    }
}
