using System;

namespace RayTracer.Imaging.Png;

[Flags]
public enum PngColorType : byte
{
    None = 0,
    PaletteUsed = 1,
    ColorUsed = 2,
    AlphaChannelUsed = 4
}