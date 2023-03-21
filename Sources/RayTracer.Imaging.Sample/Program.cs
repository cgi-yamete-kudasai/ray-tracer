using System;
using RayTracer.Imaging;
using RayTracer.Library.CLI;

ArgSwitchesProcessor.Process(args);

Console.WriteLine(ImageConverterConfiguration.Instance.Source);
Console.WriteLine(ImageConverterConfiguration.Instance.Target);
Console.WriteLine(ImageConverterConfiguration.Instance.Output);
