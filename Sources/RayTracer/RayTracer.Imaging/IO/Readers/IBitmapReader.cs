using System;
using System.IO;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.IO.Readers;

public interface IBitmapReader
{
    ReadOnlySpan<byte> MagicBytes { get; }

    Bitmap Read(Stream source);
}
