using System.Collections.Generic;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Tests.Shapes;

public class SphereTests
{
    private static readonly Vector3 Origin = new Vector3(0, 0, 0);

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void SphereIntersectionTest(Sphere sphere, Ray ray, Vector3 expPoint, bool expIntersexts)
    {
        var intersects = sphere.TryIntersect(ray, out var intersectionResult);
        
        Assert.True(intersectionResult.IsCorrect());
        Assert.Equal(expIntersexts, intersects);
        if (intersects)
        {
            Assert.Equal(expPoint, intersectionResult.Point);
        }
    }
    
    public static IEnumerable<object?[]> GetTestData()
    {
        yield return new object[] //промінь перетинає сферу в 2 точках
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(0, -3, 0)), 
            new Vector3(0, -1, 0),
            true
        };
        yield return new object?[] // промінь протилежний до минулого
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(0, 3, 0)), 
            null,
            false
        };
        yield return new object[] // перетинає сферу в 1 точці
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(-1, -2, 0)),
            new Vector3(-0.6f, -1.2f, 0),
            true
        };
        yield return new object?[] // не перетинає
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(-2, -2, 0)), 
            null,
            false
        };
        
        
        yield return new object[] // у 2 точках
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(0, 0, 1)), 
            new Vector3(0, 0, 1),
            true
        };
        yield return new object?[] // протилежний до того, що перетинає
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(0, 0, -1)), 
            null,
            false
        };
        yield return new object?[] // не перетинає
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(1, 1, 1)), 
            null,
            false
        };
    }
}