using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.Lights;

public class PointLight : ILight
{
    public ColorRGB Color { get; }

    public Vector3 Position { get; }

    public PointLight(Vector3 position, ColorRGB color)
    {
        Color = color;
        Position = position;
    }

    public PointLight(Vector3 position)
        : this(position, ColorRGB.White)
    { }

    public bool TryGetDirection(in IntersectionResult result, out Vector3 direction)
    {
        direction = Vector3.Normalize(result.Point - Position);
        return true;
    }

    public ColorRGB PaintPoint(IIntersectable shape, in IntersectionResult result)
    {
        var direction = Vector3.Normalize(result.Point - Position);
        float dot = Vector3.Dot(result.Normal, -1 * direction);
        dot = Math.Max(0, dot);
        return dot * Color;
    }
}
