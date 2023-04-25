using System;

namespace RayTracer.Imaging.Png.PngChunks;

[Flags]
public enum PngColorType : byte
{
    GreyScale = 0,
    PaletteUsed = 1 << 0,
    ColorUsed = 1 << 1,
    AlphaChannelUsed = 1 <<2,
}