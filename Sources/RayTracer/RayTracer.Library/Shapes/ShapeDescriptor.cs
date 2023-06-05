using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public readonly struct ShapeDescriptor
{
    public required IIntersectable Shape { get; init; }

    public WorldTransform Transform { get; init; }

    public ShapeDescriptor()
    {
        Transform = WorldTransform.Identity;
    }
}
