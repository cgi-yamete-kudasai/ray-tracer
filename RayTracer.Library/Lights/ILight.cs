using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Lights;

public interface ILight
{
    ColorRGB Color(IIntersectable shape, Vector3 point);
}
