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
        
        Assert.Equal(expIntersexts, intersects);
        if (intersects)
        {
            Assert.Equal(expPoint, intersectionResult.Point);
        }
    }
    
    public static IEnumerable<object?[]> GetTestData()
    {
        yield return new object?[]
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(0, 3, 0)), 
            null,
            false
        };
        yield return new object[]
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(0, -3, 0)), 
            new Vector3(0, -1, 0),
            true
        };
        yield return new object[]
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(-1, -2, 0)),
            new Vector3(-0.6f, -1.2f, 0),
            true
        };
        yield return new object?[]
        {
            new Sphere(new Vector3(0, -2, 0), 1), 
            new Ray(Origin, new Vector3(-2, -2, 0)), 
            null,
            false
        };
        
        
        yield return new object[]
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(0, 0, 1)), 
            new Vector3(0, 0, 1),
            true
        };
        yield return new object?[]
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(0, 0, -1)), 
            null,
            false
        };
        yield return new object?[]
        {
            new Sphere(new Vector3(-2, 0, 1), 2), 
            new Ray(Origin, new Vector3(1, 1, 1)), 
            null,
            false
        };
    }
}