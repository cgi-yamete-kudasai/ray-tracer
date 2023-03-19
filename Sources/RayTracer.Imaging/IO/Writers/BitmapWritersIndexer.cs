using System;
using System.Collections.Generic;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Writers;

public class BitmapWritersIndexer : ModuleTypesIndexerBase<BitmapWritersIndexer, ModuleBitmapWritersAttribute>
{
    public IReadOnlyDictionary<ImageFormat, IBitmapWriter> Writers => _writers;

    private readonly Dictionary<ImageFormat, IBitmapWriter> _writers = new();

    protected override void Process(Type type)
    {
        var writer = (IBitmapWriter)Activator.CreateInstance(type)!;
        _writers.Add(writer.Format, writer);
    }
}
