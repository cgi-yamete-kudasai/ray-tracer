using System.Drawing.Imaging;
using System.Runtime.Versioning;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;
using BM = System.Drawing.Bitmap;
using Color = System.Drawing.Color;

namespace RayTracer.Library.IO.Bitmaps.Writers;

[SupportedOSPlatform("windows")]
public class ImageBitmapWriter : IBitmapWriter
{
    public string Filename { get; }

    public ImageFormat Format { get; }

    public ImageBitmapWriter(string filename, ImageFormat? format = null)
    {
        Format = format ?? ImageFormat.Png;
        Filename = filename;
    }

    public void Write(Bitmap bitmap)
    {
        BM map = new(bitmap.Width, bitmap.Height);

        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                ColorRGB color = bitmap.Get(j, i);
                Color bmColor = Color.FromArgb((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
                map.SetPixel(j, i, bmColor);
            }
        }

        map.Save(Filename, ImageFormat.Png);
    }
}
