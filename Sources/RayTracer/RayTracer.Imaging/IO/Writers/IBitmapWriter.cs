using System.IO;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Writers;

public interface IBitmapWriter
{
    ImageFormat Format { get; }

    void Write(Stream destination, Bitmap bitmap);
}
