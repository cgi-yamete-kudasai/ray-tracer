using System.IO;
using RayTracer.Imaging;
using RayTracer.Imaging.Bmp;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Bmp.Writer;

public class BmpBitmapWriter : IBitmapWriter
{
    public ImageFormat Format => ImageFormat.Bmp;

    public void Write(Stream destination, Bitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        BmpHeader header = new((uint)width, (uint)height);
        destination.MarshalWriteStructure(header);

        int bytesToSkip = 4 - width * 3 % 4;

        if (bytesToSkip == 4)
            bytesToSkip = 0;

        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                ColorRGB color = bitmap.Get(j, i);
                
                byte r = (byte)(color.R * 255);
                byte g = (byte)(color.G * 255);
                byte b = (byte)(color.B * 255);

                destination.WriteByte(b);
                destination.WriteByte(g);
                destination.WriteByte(r);
            }

            for (int j = 0; j < bytesToSkip; j++)
                destination.WriteByte(0);
        }
    }
}
