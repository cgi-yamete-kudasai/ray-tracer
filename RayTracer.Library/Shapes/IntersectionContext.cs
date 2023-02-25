using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public readonly ref struct IntersectionContext
{
    public readonly ref readonly Ray Ray;

    public readonly ref readonly Vector3 DirectionalLight;

    public IntersectionContext(in Ray ray, in Vector3 directionalLight)
    {
        Ray = ref ray;
        DirectionalLight = ref directionalLight;
    }
}
