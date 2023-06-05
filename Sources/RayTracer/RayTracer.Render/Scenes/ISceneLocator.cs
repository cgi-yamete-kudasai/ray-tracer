namespace RayTracer.Render.Scenes;

public interface ISceneLocator
{
    Scene LocateScene(string name);
}
