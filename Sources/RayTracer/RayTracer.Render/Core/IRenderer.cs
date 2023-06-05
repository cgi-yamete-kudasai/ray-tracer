using RayTracer.Render.Scenes;

namespace RayTracer.Render.Core;

public interface IRenderer
{
    void Render(Camera camera, Scene scene);
}
