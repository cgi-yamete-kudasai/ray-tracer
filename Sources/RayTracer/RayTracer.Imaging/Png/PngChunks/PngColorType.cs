using System;

namespace RayTracer.Imaging.Png.PngChunks;

[Flags]
public enum PngColorType : byte
{
    GrayScale = 0,
    PaletteUsed = 1,
    ColorUsed = 2,
    AlphaChannelUsed = 4
}