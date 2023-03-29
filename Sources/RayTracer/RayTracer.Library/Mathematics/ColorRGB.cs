namespace RayTracer.Library.Mathematics;

public readonly struct ColorRGB
{
    public static readonly ColorRGB White = new(1, 1, 1);

    public readonly float R;

    public readonly float G;

    public readonly float B;

    public ColorRGB(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public ColorRGB(float rgb)
    {
        R = rgb;
        G = rgb;
        B = rgb;
    }

    public static ColorRGB operator *(float c, ColorRGB color)
    {
        return new(c * color.R, c * color.G, c * color.B);
    }
}
