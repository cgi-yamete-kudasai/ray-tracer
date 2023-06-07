using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Render.IO;

public class ObjMeshReader : IMeshReader
{
    private const string VERTEX_PREFIX = "v ";

    private const string FACE_PREFIX = "f ";

    private const string VERTEX_NORMAL_PREFIX = "vn ";

    public ImmutableArray<IIntersectable> Read(Stream source)
    {
        List<Vector3> vertices = new();
        List<Vector3> verticesNormals = new();
        var result = ImmutableArray.CreateBuilder<IIntersectable>();

        using StreamReader reader = new(source);

        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith(VERTEX_PREFIX))
            {
                ParseVertex(line);
                continue;
            }

            if (line.StartsWith(VERTEX_NORMAL_PREFIX))
            {
                ParseVertexNormal(line);
                continue;
            }

            if (line.StartsWith(FACE_PREFIX))
            {
                ParseFace(line);
                continue;
            }
        }

        return result.ToImmutable();

        void ParseVertexNormal(ReadOnlySpan<char> line)
        {
            line = line[VERTEX_NORMAL_PREFIX.Length..];
            var f1Span = line[..line.IndexOf(' ')];
            float f1 = float.Parse(f1Span, CultureInfo.InvariantCulture.NumberFormat);

            line = line[(line.IndexOf(' ') + 1)..];
            var f2Span = line[..line.IndexOf(' ')];
            float f2 = float.Parse(f2Span, CultureInfo.InvariantCulture.NumberFormat);

            line = line[(line.IndexOf(' ') + 1)..];
            var f3Span = line;
            float f3 = float.Parse(f3Span, CultureInfo.InvariantCulture.NumberFormat);

            Vector3 vertexNormal = Vector3.Normalize(new(f1, f2, f3));
            verticesNormals.Add(vertexNormal);
        }

        void ParseVertex(ReadOnlySpan<char> line)
        {
            line = line[VERTEX_PREFIX.Length..];
            var f1Span = line[..line.IndexOf(' ')];
            float f1 = float.Parse(f1Span, CultureInfo.InvariantCulture.NumberFormat);

            line = line[(line.IndexOf(' ') + 1)..];
            var f2Span = line[..line.IndexOf(' ')];
            float f2 = float.Parse(f2Span, CultureInfo.InvariantCulture.NumberFormat);

            line = line[(line.IndexOf(' ') + 1)..];
            var f3Span = line;
            float f3 = float.Parse(f3Span, CultureInfo.InvariantCulture.NumberFormat);

            Vector3 vertex = new(f1, f2, f3);
            vertices.Add(vertex);
        }

        void ParseFace(ReadOnlySpan<char> line)
        {
            line = line[FACE_PREFIX.Length..];
            var v1Span = line[..line.IndexOf(' ')];
            int v1 = int.Parse(v1Span[..v1Span.IndexOf('/')]);
            int vn1 = int.Parse(v1Span[(v1Span.IndexOf('/') + 2)..]);

            line = line[(line.IndexOf(' ') + 1)..];
            var v2Span = line[..line.IndexOf(' ')];
            int v2 = int.Parse(v2Span[..v2Span.IndexOf('/')]);
            int vn2 = int.Parse(v2Span[(v2Span.IndexOf('/') + 2)..]);

            line = line[(line.IndexOf(' ') + 1)..];
            var v3Span = line;
            int v3 = int.Parse(v3Span[..v3Span.IndexOf('/')]);
            int vn3 = int.Parse(v3Span[(v3Span.IndexOf('/') + 2)..]);

            TriangleMesh triangleMesh = new TriangleMesh(vertices[v1 - 1], vertices[v2 - 1], vertices[v3-1],
            verticesNormals[vn1 - 1], verticesNormals[vn2 - 1], verticesNormals[vn3-1]);
            result.Add(triangleMesh);
        }
    }
}