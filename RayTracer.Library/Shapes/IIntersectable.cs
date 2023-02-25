using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public interface IIntersectable
{
    bool TryIntersect(in IntersectionContext context, out ColorRGB color);
}
