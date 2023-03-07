using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Disc : IIntersectable
{
    public Vector3 Center { get; }
    private Vector3 _normal;
    public float Radius { get; }

    public Disc(Vector3 center, Vector3 normal, float radius)
    {
        Center = center;
        _normal = normal;
        Radius = radius;
    }

    public bool TryIntersect(in Ray ray, out Vector3 point)
    {
        float denominator = Vector3.Dot(_normal, ray.Direction);
        
        if (denominator > IntersectionHelper.INTERSECTION_TOLERANCE)
        {
            Vector3 vector = Center - ray.Origin;
            float t = Vector3.Dot(vector, _normal) / denominator;
            
            if (t >= 0)
            {
                point = ray.Origin + t * ray.Direction;
                return (point - Center).Length() <= Radius;
            }
        }
        
        point = default;
        return false;
    }

    public Vector3 GetNormal(in Vector3 point)
    {
        return _normal;
    }
}