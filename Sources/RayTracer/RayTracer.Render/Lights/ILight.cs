using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.Lights;

public interface ILight
{
    ColorRGB Color { get; }

    bool TryGetDirection(in IntersectionResult result, out Vector3 direction);

    ColorRGB PaintPoint(IIntersectable shape, in IntersectionResult result);
}
