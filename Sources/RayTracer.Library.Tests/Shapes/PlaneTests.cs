using System.Collections.Generic;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Tests.Shapes;

public class PlaneTests
{
    private static readonly Vector3 Origin = new Vector3(0, 0, 0);

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void PlaneIntersectionTest(Plane plane, Ray ray, Vector3 expPoint, bool expIntersexts)
    {
        var intersects = plane.TryIntersect(ray, out var intersectionResult);
        
        Assert.Equal(expIntersexts, intersects);
        if (intersects)
        {
            Assert.Equal(expPoint, intersectionResult.Point);
        }
    }
    
    public static IEnumerable<object?[]> GetTestData()
    {
        yield return new object?[] // промінь паралельний площині
        {
            new Plane(new Vector3(0, 0, -2), new Vector3(0, 1, 1)), 
            new Ray(Origin, new Vector3(1, 0, 0)), 
            null,
            false
        };
        yield return new object[] // перетин
        {
            new Plane(new Vector3(0, 0, -2), new Vector3(0, 1, 1)), 
            new Ray(Origin, new Vector3(0, -1, 0)), 
            new Vector3(0, -2, 0),
            true
        };
        yield return new object?[] // у протилежний бік
        {
            new Plane(new Vector3(0, 0, -2), new Vector3(0, 1, 1)), 
            new Ray(Origin, new Vector3(0, 1, 0)), 
            null,
            false
        };
        yield return new object[] // промінь лежить на площині
        {
            new Plane(new Vector3(0, 0, -2), new Vector3(0, 1, 1)), 
            new Ray(new Vector3(1, -2, 0), new Vector3(0, -1, 0)), 
            new Vector3(1, -2, 0),
            true
        };
        yield return new object?[] // промінь перетинає площину на надто далекій відстані
        {
            new Plane(new Vector3(0, 0, -2), new Vector3(0, 1, 1)), 
            new Ray(Origin, new Vector3(1, -1 * 1e-6f, 0)), 
            null,
            false
        };
    }
}