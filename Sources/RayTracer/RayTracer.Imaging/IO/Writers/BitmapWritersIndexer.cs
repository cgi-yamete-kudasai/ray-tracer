using System;
using System.Collections.Generic;
using System.Reflection;
using RayTracer.Imaging.Plugins;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Writers;

public class BitmapWritersIndexer : AnnotatedAssemblyIndexerBase<BitmapWritersIndexer, AssemblyPluginAttribute>
{
    public IReadOnlyDictionary<ImageFormat, IBitmapWriter> Writers => _writers;

    private readonly Dictionary<ImageFormat, IBitmapWriter> _writers = new();

    protected override void ProcessAnnotatedAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(IBitmapWriter)))
            {
                var writer = (IBitmapWriter)Activator.CreateInstance(type)!;
                _writers.Add(writer.Format, writer);
            }
        }
    }
}
