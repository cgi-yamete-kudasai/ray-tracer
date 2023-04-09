using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Tests.Mathematics;

public class MathHelperTests
{
    [Fact]
    public void QuadraticEquationNoRoots()
    {   
        var a = 1;
        var b = 10;
        var c = 30;

        var roots = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);
        
        Assert.Equal(0, rootsCount);
        Assert.Equal((float.NaN, float.NaN), roots);
    }
    
    [Fact]
    public void QuadraticEquationTwoRoots()
    {
        var a = 1;
        var b = 10;
        var c = 24;

        var roots = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);
        
        Assert.Equal(2, rootsCount);
        Assert.Equal((-4f, -6f), roots);
    }
    
    [Fact]
    public void QuadraticEquationOneRoot()
    {
        var a = 1;
        var b = 10;
        var c = 25;

        var roots = MathHelper.SolveQuadraticEquation(a, b, c, out int rootsCount);
        
        Assert.Equal(1, rootsCount);
        Assert.Equal((-5, -5), roots);
    }
}