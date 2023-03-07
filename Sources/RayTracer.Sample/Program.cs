using System.IO;
using RayTracer.Library.IO;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Shapes;
using RayTracer.Library.Utils;

CameraSettings settings = CameraSettings.Default with
{
    ImageHeight = 100
};

Camera camera = new(settings);

FileStream fs = File.OpenRead("../../../../../Assets/Scenes/SpheresTest.json");
Scene scene = SerializationHelper.Deserialize<Scene>(fs)!;
//scene.Shapes.Add(new Plane(new Vector3(0,0,-3), Vector3.Normalize(new Vector3(0,0,1))));
//scene.Shapes.Add(new Disc(new Vector3(0,0,-3), new Vector3(0,0,1), 0.6f));
// fs.Close();
// FileStream fsw = File.OpenWrite("../../../../../Assets/Scenes/SpheresTest.json");
// SerializationHelper.Serialize(fsw, scene);
//scene.Shapes.Add(new Sphere(new Vector3(0,0,-3), 0.6f));
// {
//   "$Type": "RayTracer.Library.Shapes.Sphere",
//   "$Value": {
//     "Radius": 0.6,
//     "Center": {
//       "X": 0,
//       "Y": 0,
//       "Z": -3
//     }
//   }
// },

Bitmap bitmap = camera.Render(scene);
ConsoleBitmapWriter writer = new();

//writer.Write(bitmap);
ImageBitmapWriter imageWriter = new("output.png");
imageWriter.Write(bitmap);