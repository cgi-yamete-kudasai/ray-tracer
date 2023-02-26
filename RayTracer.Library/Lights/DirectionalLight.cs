using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Lights;

public class DirectionalLight : ILight
{
    public ColorRGB Color { get; }

    public Vector3 Direction { get; }

    public DirectionalLight(Vector3 direction)
        : this(direction, ColorRGB.White)
    { }

    public DirectionalLight(Vector3 direction, ColorRGB color)
    {
        Direction = direction;
        Color = color;
    }

    public ColorRGB PaintPoint(IIntersectable shape, Vector3 point)
    {
        Vector3 normal = shape.GetNormal(point);
        float dot = Vector3.Dot(normal, -1 * Direction);
        dot = Math.Max(0, dot);
        return dot * Color;
    }
}
