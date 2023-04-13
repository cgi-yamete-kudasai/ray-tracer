using System;
using System.Diagnostics.CodeAnalysis;
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

        Bitmap bitmap;

        using (var sourceFs = File.OpenRead(setup.Source))
        {
            if (!TryReadImage(sourceFs, out var image))
                throw new InvalidOperationException($"Couldn't read the format of the file {setup.Source}");
         
            bitmap = image;
        }

        if (!BitmapWritersIndexer.Instance.Writers.TryGetValue(setup.TargetFormat, out var writer))
            throw new InvalidOperationException($"Format {setup.TargetFormat} is not supported for writing");

        if (!File.Exists(setup.Source))
            throw new FileNotFoundException($"Can't find source file {setup.Source}.");

        using (var destinationFs = File.OpenWrite(setup.Output))
            writer.Write(destinationFs, bitmap);
    }

    private bool TryReadImage(Stream source, [MaybeNullWhen(false)] out Bitmap image)
    {
        foreach (var reader in BitmapReadersIndexer.Instance.Readers)
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
