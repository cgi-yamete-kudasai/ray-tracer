using RayTracer.Library.Extensions;

namespace RayTracer.Library.Tests.Utils;

public class SingleComparisonTests
{
    [Fact]
    public void Equal()
    {
        float f1 = 1;
        float f2 = 0.9999999f;

        Assert.True(f1.IsEqualTo(f2));
    }

    [Fact]
    public void NotEqual()
    {
        float f1 = 1;
        float f2 = 0.9999998f;

        Assert.False(f1.IsEqualTo(f2));
    }

    [Fact]
    public void NaN()
    {
        float f1 = float.NaN;
        float f2 = float.NaN;

        Assert.True(f1.IsEqualTo(f2));
    }

    [Fact]
    public void PositiveInfinity()
    {
        float f1 = float.PositiveInfinity;
        float f2 = float.PositiveInfinity;

        Assert.True(f1.IsEqualTo(f2));
    }

    [Fact]
    public void NegativeInfinity()
    {
        float f1 = float.NegativeInfinity;
        float f2 = float.NegativeInfinity;

        Assert.True(f1.IsEqualTo(f2));
    }
}
