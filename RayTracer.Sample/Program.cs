using RayTracer.Library.IO;
using RayTracer.Library.Shapes;
using RayTracer.Library.Utils;

CameraSettings settings = CameraSettings.Default with
{
    ImageHeight = 20
};

Camera camera = new(settings);

Sphere sphere = new(new(0, 0, -3), 1);
Scene scene = new(sphere);

Bitmap bitmap = camera.Render(scene);
ConsoleBitmapWriter writer = new();

writer.Write(bitmap);
