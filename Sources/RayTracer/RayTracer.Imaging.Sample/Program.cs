using RayTracer.Imaging.Sample;
using RayTracer.Library.CLI;

// --source=example.jpg --goal-format=png [--output=output.png]

ArgSwitchesProcessor.Process(args);
new ConverterApp().Run();
