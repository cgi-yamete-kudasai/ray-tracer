using System.IO;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Writers;

public interface IBitmapWriter
{
    string Format { get; }

    void Write(Stream destination, Bitmap bitmap);
}
