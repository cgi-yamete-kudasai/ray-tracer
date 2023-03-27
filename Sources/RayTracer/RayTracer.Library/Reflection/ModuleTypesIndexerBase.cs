using System;
using System.Reflection;

namespace RayTracer.Library.Reflection;

public abstract class ModuleTypesIndexerBase<T, TAttribute> : AssemblyIndexerBase<T>
    where T : ModuleTypesIndexerBase<T, TAttribute>, new()
    where TAttribute : ModuleTypesAttribute
{
    protected abstract void Process(Type type);

    protected override void Process(Assembly assembly)
    {
        foreach (var module in assembly.Modules)
        {
            if (module.GetCustomAttribute<TAttribute>() is not { } attr)
                continue;

            foreach (var type in attr.Types)
                Process(type);
        }
    }
}
