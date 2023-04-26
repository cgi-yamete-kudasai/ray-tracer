using System;
using System.IO;
using System.Text;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Writers;

public class StringBitmapWriter : IBitmapWriter
{
    string IBitmapWriter.Format => throw new NotSupportedException();

    public void Write(Stream destination, Bitmap bitmap)
    {
        StringBuilder builder = new();

        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                char @char = ColorToChar(bitmap.Get(j, i));
                builder.Append(@char);
                builder.Append(' ');
            }

            builder.AppendLine();
        }

        using StreamWriter writer = new(destination);
        writer.Write(builder.ToString());
    }

    private static char ColorToChar(in ColorRGB color)
    {
        if (color.R == 0 && color.G == 0 && color.B == 0)
            return ' ';

        if (color.R < 0.2f && color.G < 0.2f && color.B < 0.2f)
            return '.';

        if (color.R < 0.5f && color.G < 0.5f && color.B < 0.5f)
            return '*';

        if (color.R < 0.8f && color.G < 0.8f && color.B < 0.8f)
            return 'O';

        return '#';
    }
}
