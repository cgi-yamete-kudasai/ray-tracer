using System.IO;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.IO;

public interface IMeshReader
{
    IntersectableList Read(Stream source);
}
