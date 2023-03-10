using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public interface IIntersectable
{
    bool TryIntersect(in Ray ray, out IntersectionResult result);
}