using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Utils;

public class Bitmap
{
    public int Width { get; init; }

    public int Height { get; init; }

    private readonly ColorRGB[,] _map;

    public Bitmap(int width, int height)
    {
        Width = width;
        Height = height;

        _map = new ColorRGB[Height, Width];
    }

    public void SetColor(int x, int y, in ColorRGB color)
    {
        _map[y, x] = color;
    }

    public ColorRGB Get(int x, int y)
    {
        return _map[y, x];
    }
}
