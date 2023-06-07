using RayTracer.Library.Mathematics;
using Xunit;

public class BoundingBoxTests
{
    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Inside_BoundingBox()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
        Ray ray = new Ray(new Vector3(1, 1, 1), new Vector3(-1, -1, -1));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Originating_Outside_BoundingBox()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(3, 0, 0), new Vector3(-1, 0, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(2.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Does_Not_Intersection_BoundingBox()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(3, 0, 0), new Vector3(1, 1, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Positive_X_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(-2, 0, 0), new Vector3(1, 0, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Positive_X_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(2, 0, 0), new Vector3(1, 0, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Negative_X_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(2, 0, 0), new Vector3(-1, 0, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Negative_X_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(-2, 0, 0), new Vector3(-1, 0, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Positive_Y_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, -2, 0), new Vector3(0, 1, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Positive_Y_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 2, 0), new Vector3(0, 1, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Negative_Y_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 2, 0), new Vector3(0, -1, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Negative_Y_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, -2, 0), new Vector3(0, -1, 0));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Positive_Z_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 0, -2), new Vector3(0, 0, 1));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Positive_Z_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 0, 2), new Vector3(0, 0, 1));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_True_When_Ray_Directed_Towards_Negative_Z_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 0, 2), new Vector3(0, 0, -1));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.True(result);
        Assert.Equal(1.0f, t);
    }

    [Fact]
    public void IntersectsWithRay_Should_Return_False_When_Ray_Directed_Away_From_Negative_Z_Side()
    {
        // Arrange
        BoundingBox box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Ray ray = new Ray(new Vector3(0, 0, -2), new Vector3(0, 0, -1));
        float t;

        // Act
        bool result = box.IntersectsWithRay(ray, out t);

        // Assert
        Assert.False(result);
        Assert.Equal(-1.0f, t);
    }
}
