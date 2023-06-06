using System;
using RayTracer.DependencyInjection;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;
using RayTracer.Render.IO;
using RayTracer.Render.Lights;
using RayTracer.Render.Scenes;

namespace RayTracer.Render.Sample;

public class InMemorySceneLocator : ISceneLocator
{
    private const string COW_OBJ_FILE_NAME = "cow.obj";

    [Service]
    private readonly IMeshReader _meshReader = null!;

    public Scene LocateScene(string name)
    {
        return name switch
        {
            "cows" => TwoCows(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Scene TwoCows()
    {
        var builder = Scene.CreateBuilder(_meshReader);

        // cows
        builder.AddMeshFromFile(
            COW_OBJ_FILE_NAME,
            WorldTransform.Identity
                .Translate(new(-.6f, 0, 0)));

        builder.AddMeshFromFile(
            COW_OBJ_FILE_NAME,
            WorldTransform.Identity
                .Translate(new(-.6f, 0, 0))
                .RotateZ((float)Math.PI));

        // spheres
        builder.AddIntersectable(new()
        {
            Shape = new Sphere(new(-.75f, 0, .5f), .1f)
        });

        builder.AddIntersectable(new()
        {
            Shape = new Sphere(new(-.375f, 0, .5f), .1f)
        });

        builder.AddIntersectable(new()
        {
            Shape = new Sphere(new(0, 0, .5f), .1f)
        });

        builder.AddIntersectable(new()
        {
            Shape = new Sphere(new(.375f, 0, .5f), .1f)
        });

        builder.AddIntersectable(new()
        {
            Shape = new Sphere(new(.75f, 0, .5f), .1f)
        });

        // floors
        builder.AddIntersectable(new()
        {
            Shape = new Disc(new(-.6f, 0, -.325f), new(0, 0, 1), .5f)
        });

        builder.AddIntersectable(new()
        {
            Shape = new Disc(new(.6f, 0, -.325f), new(0, 0, 1), .5f)
        });

        // lights
        builder.AddLight(new PointLight(new(0, -1f, 0)));

        return builder.Build();
    }
}
