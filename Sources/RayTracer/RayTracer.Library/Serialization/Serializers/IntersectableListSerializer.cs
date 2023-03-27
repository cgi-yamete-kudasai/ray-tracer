using System.Collections.Immutable;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Serialization.Serializers;

public class IntersectableListSerializer : ProxySerializer<IntersectableListSerializer, IntersectableList,
    ImmutableArray<IIntersectable>>
{
    protected override ISerializer<ImmutableArray<IIntersectable>> ProxyTypeSerializer =>
        ImmutableArraySerializer<IIntersectable>.Instance;

    protected override ImmutableArray<IIntersectable> Convert(IntersectableList? value)
    {
        if (value is null)
        {
            return ImmutableArray<IIntersectable>.Empty;
        }

        return value.ToImmutableArray();
    }

    protected override IntersectableList? Convert(ImmutableArray<IIntersectable> value)
    {
        return new IntersectableList(value);
    }
}