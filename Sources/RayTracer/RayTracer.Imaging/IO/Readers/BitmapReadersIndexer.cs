using System;
using System.Collections.Generic;
using System.Reflection;
using RayTracer.Imaging.Plugins;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Readers;

public class BitmapReadersIndexer : AnnotatedAssemblyIndexerBase<BitmapReadersIndexer, AssemblyPluginAttribute>
{
    public IReadOnlyList<IBitmapReader> Readers => _readers;

    private readonly List<IBitmapReader> _readers = new();

    protected override void ProcessAnnotatedAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(IBitmapReader)))
            {
                var reader = (IBitmapReader)Activator.CreateInstance(type)!;
                _readers.Add(reader);
            }
        }
    }
}
