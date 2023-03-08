using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Library.IO;

public class ConsoleBitmapWriter : IBitmapWriter
{
    public void Write(Bitmap bitmap)
    {
        Console.Clear();

        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                char @char = ColorToChar(bitmap.Get(j, i));
                Console.Write(@char);
                Console.Write(' ');
            }

            Console.WriteLine();
        }
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
