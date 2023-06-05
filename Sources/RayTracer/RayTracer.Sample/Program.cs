using System;
using System.IO;
using System.Text;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Library.Serialization;
using RayTracer.Library.Utils;
using RayTracer.Render.Core;
using RayTracer.Render.Scenes;

CameraSettings settings = CameraSettings.Default with
{
    ImageHeight = 20
};

Camera camera = new(settings);

//FileStream fs = File.OpenRead("../../../../../Assets/Scenes/SpheresTest.json");
//Scene scene = SerializationHelper.Deserialize<Scene>(fs)!;

Scene scene = null!;

BitmapRenderer renderer = new();
renderer.Render(camera, scene);
Bitmap bitmap = renderer.GetResult();

StringBitmapWriter writer = new();

MemoryStream ms = new();
writer.Write(ms, bitmap);

string image = Encoding.UTF8.GetString(ms.GetBuffer());
Console.WriteLine(image);
