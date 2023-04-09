using System;
using System.IO;
using System.Text;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.Serialization;
using RayTracer.Library.Utils;
using RayTracer.Render.Core;

CameraSettings settings = CameraSettings.Default with
{
    ImageHeight = 20
};

Camera camera = new(settings);

FileStream fs = File.OpenRead("../../../../../Assets/Scenes/SpheresTest.json");
Scene scene = SerializationHelper.Deserialize<Scene>(fs)!;

Bitmap bitmap = camera.Render(scene);

StringBitmapWriter writer = new();

MemoryStream ms = new();
writer.Write(ms, bitmap);

string image = Encoding.UTF8.GetString(ms.GetBuffer());
Console.WriteLine(image);
