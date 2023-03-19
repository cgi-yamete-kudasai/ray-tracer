using System;

namespace RayTracer.Library.Reflection;

[AttributeUsage(AttributeTargets.Module)]
public abstract class ModuleTypesAttribute : Attribute
{
    public Type[] Types { get; }

    protected ModuleTypesAttribute(params Type[] types)
    {
        Types = types;
    }
}
