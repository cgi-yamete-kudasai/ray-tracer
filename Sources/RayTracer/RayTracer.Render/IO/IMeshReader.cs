using System.Collections.Immutable;
using System.IO;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.IO;

public interface IMeshReader
{
    ImmutableArray<IIntersectable> Read(Stream source);
}
