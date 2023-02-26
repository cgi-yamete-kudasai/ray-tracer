using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Serialization.Serializers;

public class ColorRGBSerializer : ProxySerializer<ColorRGBSerializer, ColorRGB, Vector3>
{
    protected override ISerializer<Vector3> ProxyTypeSerializer => Vector3Serializer.Instance;

    protected override Vector3 Convert(ColorRGB value)
    {
        return new(value.R, value.G, value.B);
    }

    protected override ColorRGB Convert(Vector3 value)
    {
        return new(value.X, value.Y, value.Z);
    }
}
