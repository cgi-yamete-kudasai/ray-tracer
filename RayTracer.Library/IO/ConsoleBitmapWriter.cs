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
        if (color.R > 0.5f && color.G > 0.5f && color.B > 0.5f)
            return '#';

        return '.';
    }
}
