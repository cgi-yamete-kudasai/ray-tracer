using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Lights;

public interface ILight
{
    ColorRGB Color { get; }

    ColorRGB PaintPoint(IIntersectable shape, in IntersectionResult result);
}
