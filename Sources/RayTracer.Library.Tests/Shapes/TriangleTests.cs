using System.Collections.Generic;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Tests.Shapes;

public class TriangleTests
{
    private static readonly Vector3 Origin = new Vector3(0, 0, 0);

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void PlaneIntersectionTest(Triangle triangle, Ray ray, Vector3 expPoint, bool expIntersects)
    {
        var intersects = triangle.TryIntersect(ray, out var intersectionResult);

        Assert.Equal(expIntersects, intersects);
        if (intersects)
        {
            Assert.Equal(expPoint, intersectionResult.Point);
        }
    }

    public static IEnumerable<object?[]> GetTestData()
    {
        yield return new object?[] // промінь перетинає вершину
        {
            new Triangle(new Vector3(1, 0, 0), new Vector3(0, 0, 3), new Vector3(0, 2, 0)),
            new Ray(Origin, new Vector3(1, 0, 0)),
            new Vector3(1, 0, 0),
            true
        };
        yield return new object?[] // промінь перетинає сторону
        {
            new Triangle(new Vector3(1, 0, 0), new Vector3(0, 0, 3), new Vector3(0, 2, 0)),
            new Ray(Origin, new Vector3(0.5f, 0, 1.5f)),
            new Vector3(0.5f, 0, 1.5f),
            true
        };
        yield return new object?[] // промінь перетинає точку всередині трикутника
        {
            new Triangle(new Vector3(0, 0, -1), new Vector3(1, 0, 2), new Vector3(-1, 0, 3)),
            new Ray(new Vector3(0, -2, 0), new Vector3(0, 1, 0)),
            new Vector3(0, 0, 0),
            true
        };
        yield return new object?[] // промінь перетинає точку всередині трикутника (під кутом)
        {
            new Triangle(new Vector3(0, 0, -1), new Vector3(1, 0, 2), new Vector3(-1, 0, 3)),
            new Ray(new Vector3(0, -2, 0), new Vector3(0, 1, 1)),
            new Vector3(0, 0, 2),
            true
        };
        yield return new object?[] // промінь спрямований у протилежний бік
        {
            new Triangle(new Vector3(1, 0, 0), new Vector3(0, 0, 3), new Vector3(0, 2, 0)),
            new Ray(Origin, new Vector3(0.5f, 0, -1.5f)),
            null,
            false
        };
    }
}