using System.Collections.Immutable;
using RayTracer.Library.Lights;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Utils;

public class Scene : ISerializable<Scene>
{
    public ImmutableArray<IIntersectable> Shapes { get; }

    public ImmutableArray<ILight> Lights { get; }

    public Scene(ImmutableArray<IIntersectable> shapes, ImmutableArray<ILight> lights)
    {
        Shapes = shapes;
        Lights = lights;
    }

    public Scene(ILight light, params IIntersectable[] shapes)
    {
        Shapes = shapes.ToImmutableArray();
        Lights = ImmutableArray<ILight>.Empty.Add(light);
    }

    static ISerializer<Scene> ISerializable<Scene>.Serializer => SceneSerializer.Instance;
}
