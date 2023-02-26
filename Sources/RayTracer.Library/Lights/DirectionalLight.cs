using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Lights;

public class DirectionalLight : ILight, ISerializable<DirectionalLight>
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
        Vector3 normal = Vector3.Normalize(shape.GetNormal(point));
        float dot = Vector3.Dot(normal, -1 * Direction);
        dot = Math.Max(0, dot);
        return dot * Color;
    }

    static ISerializer<DirectionalLight> ISerializable<DirectionalLight>.Serializer => DirectionalLightSerializer.Instance;
}
