using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Utils;

public interface ITransformable
{
    void Transform(WorldTransform wt);
}
