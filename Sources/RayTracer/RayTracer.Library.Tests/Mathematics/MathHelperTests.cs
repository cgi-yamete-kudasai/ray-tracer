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

    [Fact]
    public void MatrixMultiplicationSquareMatrices()
    {
        float[,] matrix1 = new float[3, 3]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };
        
        float[,] matrix2 = new float[3, 3]
        {
            { 10, 11, 12 },
            { 13, 14, 15 },
            { 16, 17, 18 }
        };
        
        float[,] expected = new float[3, 3]
        {
            { 84, 90, 96 },
            { 201, 216, 231 },
            { 318, 342, 366 }
        };
        
        float[,] actual = matrix1.MultiplyByMatrix(matrix2);
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MatrixMultiplicationNonSquareMatrices()
    {
        float[,] matrix1 = new float[3, 2]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        };
        
        float[,] matrix2 = new float[2, 3]
        {
            { 7, 8, 9 },
            { 10, 11, 12 }
        };
        
        float[,] expected = new float[3, 3]
        {
            { 27, 30, 33 },
            { 61, 68, 75 },
            { 95, 106, 117 }
        };
        
        float[,] actual = matrix1.MultiplyByMatrix(matrix2);
        
        Assert.Equal(expected, actual);
    }
}