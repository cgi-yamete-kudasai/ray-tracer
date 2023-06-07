using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Shapes;
using RayTracer.Render.IO;
using RayTracer.Render.Lights;

namespace RayTracer.Render.Scenes;

public class Scene
{
    public ImmutableArray<ShapeDescriptor> Shapes { get; }

    public ImmutableArray<ILight> Lights { get; }

    public Scene(ImmutableArray<ShapeDescriptor> shapes, ImmutableArray<ILight> lights)
    {
        Shapes = shapes;
        Lights = lights;
    }

    public static Builder CreateBuilder(IMeshReader reader) => new(reader);

    public class Builder
    {
        private readonly IMeshReader _reader;

        private readonly List<(string File, WorldTransform Translation)> _meshFiles;

        private readonly ImmutableArray<ShapeDescriptor>.Builder _shapes;

        private readonly ImmutableArray<ILight>.Builder _lights;

        internal Builder(IMeshReader reader)
        {
            _reader = reader;
            _meshFiles = new();
            _shapes = ImmutableArray.CreateBuilder<ShapeDescriptor>();
            _lights = ImmutableArray.CreateBuilder<ILight>();
        }

        public Builder AddLight(ILight light)
        {
            _lights.Add(light);
            return this;
        }

        public Builder AddIntersectable(ShapeDescriptor intersectable)
        {
            _shapes.Add(intersectable);
            return this;
        }

        public Builder AddMeshFromFile(string fileName, WorldTransform transform)
        {
            _meshFiles.Add((fileName, transform));
            return this;
        }

        public Scene Build()
        {
            // process mesh files.
            foreach (var (meshFile, transform) in _meshFiles)
            {
                using var stream = File.OpenRead(meshFile);
                var faces = _reader.Read(stream);
                var mesh = new IntersectableList(faces);
                _shapes.Add(new()
                {
                    Shape = mesh,
                    Transform = transform
                });
            }

            var shapes = _shapes.ToImmutable();
            var lights = _lights.ToImmutable();

            return new(shapes, lights);
        }
    }
}
