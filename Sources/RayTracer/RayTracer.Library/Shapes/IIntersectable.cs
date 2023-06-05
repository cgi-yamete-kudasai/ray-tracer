using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Shapes;

public interface IIntersectable : ITransformable
{
    BoundingBox BB { get; }

    bool TryIntersect(in Ray ray, out IntersectionResult result);
}