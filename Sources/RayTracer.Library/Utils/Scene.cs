using System.Collections.Immutable;
using System.Linq;
using RayTracer.Library.Lights;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Utils;

public class Scene : ISerializable<Scene>
{
    public IntersectableList Shapes { get; }

    public ImmutableArray<ILight> Lights { get; }

    public Scene(ImmutableArray<IIntersectable> shapes, ImmutableArray<ILight> lights)
    {
        Shapes = new(shapes.ToArray());
        Lights = lights;
    }

    public Scene(ILight light, params IIntersectable[] shapes)
    {
        Shapes = new(shapes.ToArray());
        Lights = ImmutableArray<ILight>.Empty.Add(light);
    }

    static ISerializer<Scene> ISerializable<Scene>.Serializer => SceneSerializer.Instance;
}