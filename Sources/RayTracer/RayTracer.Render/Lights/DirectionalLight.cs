using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.Lights;

public class DirectionalLight : ILight, ISerializable<DirectionalLight>
{
    public ColorRGB Color { get; }

    public Vector3 Direction { get; }

    public DirectionalLight(Vector3 direction)
        : this(direction, ColorRGB.White)
    {
    }

    public DirectionalLight(Vector3 direction, ColorRGB color)
    {
        Direction = Vector3.Normalize(direction);
        Color = color;
    }

    public bool TryGetDirection(in IntersectionResult result, out Vector3 direction)
    {
        direction = Direction;
        return true;
    }

    public ColorRGB PaintPoint(IIntersectable shape, in IntersectionResult result)
    {
        float dot = Vector3.Dot(result.Normal, -1 * Direction);
        dot = Math.Max(0, dot);
        return dot * Color;
    }

    static ISerializer<DirectionalLight> ISerializable<DirectionalLight>.Serializer =>
        DirectionalLightSerializer.Instance;
}
