using System;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Readers;

public class ModuleBitmapReadersAttribute : ModuleTypesAttribute
{
    public ModuleBitmapReadersAttribute(Type[] types)
        : base(types)
    { }
}
