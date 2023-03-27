using System;
using System.Reflection;

namespace RayTracer.Library.Reflection;

public abstract class AnnotatedAssemblyIndexerBase<T, TAttribute> : AssemblyIndexerBase<T>
    where T : AnnotatedAssemblyIndexerBase<T, TAttribute>, new()
    where TAttribute : Attribute
{
    protected override void Process(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<TAttribute>() is null)
            return;

        ProcessAnnotatedAssembly(assembly);
    }

    protected abstract void ProcessAnnotatedAssembly(Assembly assembly);
}
