using System;

namespace RayTracer.IO.CLI;

[AttributeUsage(AttributeTargets.Module)]
public class ModuleArgSwitchesAttribute : Attribute
{
    public Type[] Types { get; }

    public ModuleArgSwitchesAttribute(params Type[] types)
    {
        Types = types;
    }
}
