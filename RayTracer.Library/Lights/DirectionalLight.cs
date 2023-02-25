using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Lights;

public class DirectionalLight : ILight
{
    public Vector3 Direction { get; }

    public DirectionalLight(Vector3 direction)
    {
        Direction = direction;
    }

    public ColorRGB Color(IIntersectable shape, Vector3 point)
    {
        Vector3 normal = shape.GetNormal(point);
        float dot = Vector3.Dot(normal, -1 * Direction);
        return new ColorRGB(dot);
    }
}
