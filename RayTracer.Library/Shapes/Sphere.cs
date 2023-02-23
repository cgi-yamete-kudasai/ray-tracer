using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Sphere : IIntersectable
{
    public Vector3 Center { get; init; }

    public float Radius { get; init; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool TryIntersect(in Ray ray, out ColorRGB color)
    {
        Vector3 k = ray.Origin - Center;
        float a = Vector3.Dot(ray.Direction, ray.Direction);
        float b = 2 * Vector3.Dot(k, ray.Direction);
        float c = Vector3.Dot(k, k) - Radius * Radius;
        float discriminant = b * b - 4 * a * c;

        if (discriminant >= 0)
        {
            color = new ColorRGB(1, 1, 1);
            return true;
        }

        color = new ColorRGB(0, 0, 0);
        return false;
    }
}
