using System.Collections.Generic;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Tests.Shapes;

public class DiscTests
{
    private static readonly Vector3 Origin = new Vector3(0, 0, 0);

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void DiscIntersectionTest(Disc disc, Ray ray, Vector3 expPoint, bool expIntersects)
    {
        var intersects = disc.TryIntersect(ray, out var intersectionResult);

        Assert.Equal(expIntersects, intersects);
        if (intersects)
        {
            Assert.True(intersectionResult.IsCorrect());
            Assert.Equal(expPoint, intersectionResult.Point);
        }
    }

    public static IEnumerable<object?[]> GetTestData()
    {
        yield return new object?[] // промінь перетинає диск
        {
            new Disc(new Vector3(0, 4, 0), new Vector3(0, -1, -1), 1),
            new Ray(Origin, new Vector3(0, 1, 0)),
            new Vector3(0, 4, 0),
            true
        };
        yield return new object?[] // промінь напрямлений у протилежний бік
        {
            new Disc(new Vector3(0, 4, 0), new Vector3(0, -1, -1), 1),
            new Ray(Origin, new Vector3(0, -1, 0)),
            null,
            false
        };
        yield return new object?[] // промінь перетинає контур диска
        {
            new Disc(new Vector3(0, 4, 0), new Vector3(0, -1, -1), 1),
            new Ray(Origin, new Vector3(1, 4, 0)),
            new Vector3(1, 4, 0),
            true
        };
        yield return new object?[] // промінь лежить у диску
        {
            new Disc(new Vector3(0, 4, 0), new Vector3(0, -1, -1), 1),
            new Ray(new Vector3(0, 4, 0), new Vector3(1, 0, 0)),
            null,
            false
        };
        yield return new object?[] // промінь не перетинає диск
        {
            new Disc(new Vector3(0, 4, 0), new Vector3(0, -1, -1), 1),
            new Ray(Origin, new Vector3(2, 4, 0)),
            null,
            false
        };
    }
}