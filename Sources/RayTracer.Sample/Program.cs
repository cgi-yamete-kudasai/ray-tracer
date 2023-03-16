using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using CommandLine;
using RayTracer.Library.IO;
using RayTracer.Library.IO.Bitmaps.Writers;
using RayTracer.Library.Serialization;
using RayTracer.Library.Utils;
using RayTracer.Sample.CommandArgsParser;

bool inputArgsAreCorrect = true;
StringBuilder errorsSB = new StringBuilder();
errorsSB.Append("Error: ");

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(options =>
{
    if (!CommandArgsParser.ParseSourcePath(options.SourcePath, out string errorMessage))
    {
        errorsSB.Append(errorMessage);
        inputArgsAreCorrect = false;
    }
});

if (!inputArgsAreCorrect)
{
    Console.WriteLine(errorsSB.ToString());
}

// CameraSettings settings = CameraSettings.Default with
// {
//     ImageHeight = 20
// };
//
// Camera camera = new(settings);
//
// FileStream fs = File.OpenRead("../../../../../Assets/Scenes/SpheresTest.json");
// Scene scene = SerializationHelper.Deserialize<Scene>(fs)!;
//
// Bitmap bitmap = camera.Render(scene);
//
// ConsoleBitmapWriter writer = new();
// writer.Write(bitmap);
