using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class Cube : IIntersectable
{
    public BoundingBox BB => new(Min, Max);
    
    public Vector3 Min { get; }
    public Vector3 Max { get; }

    public Cube(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }

    public void Transform(WorldTransform wt)
    {
        throw new System.NotImplementedException();
    }

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        throw new System.NotImplementedException();
    }
}