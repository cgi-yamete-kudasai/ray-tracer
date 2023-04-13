using System;
using System.IO;
using System.Runtime.Versioning;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;
using SD = System.Drawing;

namespace RayTracer.Imaging.IO.Writers;

[SupportedOSPlatform("windows")]
public class ImageBitmapWriter : IBitmapWriter
{
    public SD.Imaging.ImageFormat Format { get; }

    string IBitmapWriter.Format => throw new NotSupportedException();

    public ImageBitmapWriter(SD.Imaging.ImageFormat? format = null)
    {
        Format = format ?? SD.Imaging.ImageFormat.Png;
    }

    public void Write(Stream destination, Bitmap bitmap)
    {
        SD.Bitmap map = new(bitmap.Width, bitmap.Height);

        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                ColorRGB color = bitmap.Get(j, i);
                SD.Color bmColor = SD.Color.FromArgb((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
                map.SetPixel(j, i, bmColor);
            }
        }

        map.Save(destination, SD.Imaging.ImageFormat.Png);
    }
}
