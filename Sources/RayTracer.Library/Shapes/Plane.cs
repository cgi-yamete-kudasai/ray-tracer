using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Plane : IIntersectable
{
    public Vector3 BasePoint { get; }
    private Vector3 _normal;

    public Plane(Vector3 basePoint, Vector3 normal)
    {
        BasePoint = basePoint;
        _normal = normal;
    }

    public bool TryIntersect(in Ray ray, out Vector3 point)
    {
        float denominator = Vector3.Dot(_normal, ray.Direction);

        if (denominator > IntersectionHelper.INTERSECTION_TOLERANCE)
        {
            Vector3 vector = BasePoint - ray.Origin;
            float t = Vector3.Dot(vector, _normal) / denominator;

            if (t >= 0)
            {
                point = ray.Origin + t * ray.Direction;
                return true;
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