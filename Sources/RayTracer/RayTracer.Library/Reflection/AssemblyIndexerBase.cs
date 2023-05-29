using System;
using System.Reflection;

namespace RayTracer.Library.Reflection;

public abstract class AssemblyIndexerBase<T>
    where T : AssemblyIndexerBase<T>
{
    protected AssemblyIndexerBase()
    {
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            Process(a);

        AppDomain.CurrentDomain.AssemblyLoad += (_, args) => Process(args.LoadedAssembly);
    }

    protected abstract void Process(Assembly assembly);
}
