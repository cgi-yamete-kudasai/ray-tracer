using System;
using RayTracer.Library.Reflection;

namespace RayTracer.Library.CLI;

public class ModuleArgSwitchesAttribute : ModuleTypesAttribute
{
    public ModuleArgSwitchesAttribute(params Type[] types)
        : base(types)
    { }
}
