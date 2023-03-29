using System;
using System.IO;
using RayTracer.Imaging.IO.Readers;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Imaging.Sample.Configuration;
using RayTracer.Library.Utils;

namespace RayTracer.Imaging.Sample;

public class ConverterApp
{
    public void Run()
    {
        ImageConverterSetup setup = new(ImageConverterConfiguration.Instance);
        new PluginLoader().LoadPlugins();

        if (!BitmapReadersIndexer.Instance.Readers.TryGetValue(setup.SourceFormat, out var reader))
            throw new InvalidOperationException($"Format {setup.SourceFormat} is not supported for reading");

        if (!BitmapWritersIndexer.Instance.Writers.TryGetValue(setup.SourceFormat, out var writer))
            throw new InvalidOperationException($"Format {setup.SourceFormat} is not supported for writing");

        if (!File.Exists(setup.Source))
            throw new FileNotFoundException($"Can't find source file {setup.Source}.");

        Bitmap bitmap;

        using (var sourceFs = File.OpenRead(setup.Source))
            bitmap = reader.Read(sourceFs);

        using (var destinationFs = File.OpenWrite(setup.Output))
            writer.Write(destinationFs, bitmap);
    }
}
