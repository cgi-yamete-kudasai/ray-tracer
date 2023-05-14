using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using RayTracer.DependencyInjection;
using RayTracer.Imaging.IO.Readers;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Imaging.Plugins;
using RayTracer.Imaging.Sample.Configuration;
using RayTracer.Library.CLI;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.Sample;

public class ConverterApp : IArgSwitchesProvider
{
    [Service]
    private readonly ArgSwitchesProcessor _switchesProcessor = null!;

    [Service]
    private readonly BitmapReadersIndexer _readersIndexer = null!;

    [Service]
    private readonly BitmapWritersIndexer _writersIndexer = null!;
    
    [Service]
    private readonly ImageConverterConfiguration _configuration = null!;

    private readonly string[] _args;

    IReadOnlyList<object> IArgSwitchesProvider.Listeners => new[] { _configuration };

    public ConverterApp(string[] args)
    {
        _args = args;
    }

    public void Run()
    {
        _switchesProcessor.Process(_args, this);

        new PluginLoader().LoadPlugins();

        ImageConverterSetup setup = new(_configuration);

        Bitmap bitmap;

        using (var sourceFs = File.OpenRead(setup.Source))
        {
            if (!TryReadImage(sourceFs, out var image))
                throw new InvalidOperationException($"The file {setup.Source} has unsupported format");
         
            bitmap = image;
        }

        if (!_writersIndexer.Writers.TryGetValue(setup.TargetFormat, out var writer))
            throw new InvalidOperationException($"Format {setup.TargetFormat} is not supported for writing");

        if (!File.Exists(setup.Source))
            throw new FileNotFoundException($"Can't find source file {setup.Source}.");

        using var destinationFs = File.OpenWrite(setup.Output);
        writer.Write(destinationFs, bitmap);
    }

    private bool TryReadImage(Stream source, [MaybeNullWhen(false)] out Bitmap image)
    {
        foreach (var reader in _readersIndexer.Readers)
        {
            if (IsCorrectFormat(reader))
            {
                image = reader.Read(source);
                return true;
            }
        }

        image = null;
        return false;

        bool IsCorrectFormat(IBitmapReader reader)
        {
            byte[] magicBytes = new byte[reader.MagicBytes.Length];
            
            if (source.Read(magicBytes) < magicBytes.Length)
                return false;

            source.Seek(0, SeekOrigin.Begin);

            return reader.MagicBytes.SequenceEqual(magicBytes);
        }
    }
}
