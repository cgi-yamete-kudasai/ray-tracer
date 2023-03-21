using System;
using System.Collections.Generic;
using System.Reflection;
using RayTracer.Library.Reflection;

namespace RayTracer.Imaging.IO.Readers;

public class BitmapReadersIndexer : ModuleTypesIndexerBase<BitmapReadersIndexer, ModuleBitmapReadersAttribute>
{
    public IReadOnlyDictionary<ImageFormat, IBitmapReader> Readers => _readers;

    private readonly Dictionary<ImageFormat, IBitmapReader> _readers = new();

    protected override void Process(Type type)
    {
        var reader = (IBitmapReader)Activator.CreateInstance(type)!;
        _readers.Add(reader.Format, reader);
    }
}
