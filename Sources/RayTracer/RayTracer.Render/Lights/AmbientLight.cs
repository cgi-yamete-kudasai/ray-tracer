using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.Lights;

public class AmbientLight : ILight
{
    public ColorRGB Color { get; }

    public AmbientLight(ColorRGB color)
    {
        Color = color;
    }
    
    public AmbientLight()
        : this(ColorRGB.White)
    { }

    public bool TryGetDirection(in IntersectionResult result, out Vector3 direction)
    {
        direction = default;
        return false;
    }

    public ColorRGB PaintPoint(IIntersectable shape, in IntersectionResult result)
    {
        return Color;
    }
}
