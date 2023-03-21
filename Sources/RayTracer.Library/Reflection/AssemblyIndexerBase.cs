using RayTracer.Library.Utils;
using System;
using System.Reflection;

namespace RayTracer.Library.Reflection;

public abstract class AssemblyIndexerBase<T> : Singleton<T>
    where T : AssemblyIndexerBase<T>, new()
{
    protected AssemblyIndexerBase()
    {
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            Process(a);

        AppDomain.CurrentDomain.AssemblyLoad += (_, args) => Process(args.LoadedAssembly);
    }

    protected abstract void Process(Assembly assembly);
}
