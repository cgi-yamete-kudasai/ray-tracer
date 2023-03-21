using System.IO;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Readers;

public interface IBitmapReader
{
    ImageFormat Format { get; }

    Bitmap Read(Stream source);
}
