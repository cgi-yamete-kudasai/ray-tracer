namespace RayTracer.Library.Mathematics;

public readonly struct Ray
{
    public readonly Vector3 Origin;

    public readonly Vector3 Direction;

    public Ray(in Vector3 origin, in Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Ray Transform(WorldTransform wt)
    {
        Vector3 origin = Origin.Transform(wt);
        Vector3 direction = Direction.Transform(wt);
        
        return new(origin, direction);
    }
}
