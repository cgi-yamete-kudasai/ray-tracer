using System.IO;
using RayTracer.Library.IO;
using RayTracer.Library.Serialization;
using RayTracer.Library.Utils;

CameraSettings settings = CameraSettings.Default with
{
    ImageHeight = 20
};

Camera camera = new(settings);

FileStream fs = File.OpenRead("../../../../../Assets/Scenes/SpheresTest.json");
Scene scene = SerializationHelper.Deserialize<Scene>(fs)!;

Bitmap bitmap = camera.Render(scene);

ConsoleBitmapWriter writer = new();
writer.Write(bitmap);
