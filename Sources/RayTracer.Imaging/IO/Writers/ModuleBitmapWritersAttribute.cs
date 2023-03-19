using System;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Writers;

public class ModuleBitmapWritersAttribute : ModuleTypesAttribute
{
    public ModuleBitmapWritersAttribute(Type[] types)
        : base(types)
    { }
}
