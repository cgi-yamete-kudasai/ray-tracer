using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Disc : IIntersectable
{
    public Vector3 Center;
    public Vector3 Normal;
    public float Radius;

    public Disc(Vector3 center, Vector3 normal, float radius)
    {
        Center = center;
        Normal = normal;
        Radius = radius;
    }

    public bool TryIntersect(in Ray ray, out Vector3 point)
    {
        float denominator = Vector3.Dot(Normal, ray.Direction);
        if (denominator > IIntersectable.TOLERANCE)
        {
            Vector3 vector = Center - ray.Origin;
            float t = Vector3.Dot(vector, Normal) / denominator;
            if (t >= 0)
            {
                point = ray.Origin + t * ray.Direction;
                return (point - Center).Length <= Radius;
            }
        }
        
        point = new Vector3();
        return false;
    }

    public Vector3 GetNormal(in Vector3 point)
    {
        return Normal;
    }
}