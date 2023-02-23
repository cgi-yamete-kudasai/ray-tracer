using RayTracer.Library.Extensions;

namespace RayTracer.Library.Tests.Utils;

public class SingleComparisonTests
{
    [Fact]
    public void Equal()
    {
        float f1 = 1;
        float f2 = 0.99999999f;

        Assert.True(f1.IsEqualTo(f2));
    }

    [Fact]
    public void NotEqual()
    {
        float f1 = 1;
        float f2 = 0.9999999f;

        Assert.False(f1.IsEqualTo(f2));
    }
}
