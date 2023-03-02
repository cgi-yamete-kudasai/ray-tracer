using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Plane : IIntersectable
{
    public Vector3 BasePoint;
    public Vector3 Normal;

    public Plane(Vector3 basePoint, Vector3 normal)
    {
        BasePoint = basePoint;
        Normal = normal;
    }

    public bool TryIntersect(in Ray ray, out Vector3 point)
    {
        float denominator = Vector3.Dot(Normal, ray.Direction);
        if (denominator > IIntersectable.TOLERANCE)
        {
            Vector3 vector = BasePoint - ray.Origin;
            float t = Vector3.Dot(vector, Normal) / denominator;
            if (t >= 0)
            {
                point = ray.Origin + t * ray.Direction;
                return true;
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