using RayTracer.Library.IO;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;
using RayTracer.Library.Utils;

CameraSettings settings = new CameraSettings(16f / 9f, 20, 2, 1, Vector3.Zero);
Camera camera = new(settings);

Sphere sphere = new(new(0, 0, -3), 1);
Scene scene = new(sphere);

Bitmap bitmap = camera.Render(scene);
ConsoleBitmapWriter writer = new();

writer.Write(bitmap);
