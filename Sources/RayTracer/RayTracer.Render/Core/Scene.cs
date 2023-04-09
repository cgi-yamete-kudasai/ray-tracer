using System.Collections.Immutable;
using RayTracer.Library.Serialization;
using RayTracer.Library.Shapes;
using RayTracer.Render.Lights;

namespace RayTracer.Render.Core;

public class Scene : ISerializable<Scene>
{
    public ImmutableArray<IIntersectable> Shapes { get; }

    public ImmutableArray<ILight> Lights { get; }

    public Scene(ImmutableArray<IIntersectable> shapes, ImmutableArray<ILight> lights)
    {
        Shapes = shapes;
        Lights = lights;
    }

    static ISerializer<Scene> ISerializable<Scene>.Serializer => SceneSerializer.Instance;
}
