using System.Collections.Immutable;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Utils;

public class Scene
{
    public ImmutableArray<IIntersectable> Shapes { get; }

    public Scene(ImmutableArray<IIntersectable> shapes)
    {
        Shapes = shapes;
    }

    public Scene(params IIntersectable[] shapes)
    {
        Shapes = shapes.ToImmutableArray();
    }
}
