using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Serialization.Serializers;

public class SphereSerializer : SerializerBase<SphereSerializer, Sphere>
{
    public override void Serialize(Utf8JsonWriter writer, Sphere? value)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(value.Radius));
        writer.WriteNumberValue(value.Radius);

        writer.WritePropertyName(nameof(value.Center));
        Vector3Serializer.Instance.Serialize(writer, value.Center);

        writer.WriteEndObject();
    }

    public override Sphere? Deserialize(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
            return null;

        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead("Radius"u8);
        reader.EnsureToken(JsonTokenType.Number);
        float radius = reader.GetSingle();
        reader.Read();

        reader.EnsurePropertyAndRead("Center"u8);
        Vector3 center = Vector3Serializer.Instance.Deserialize(ref reader);
        reader.Read();

        return new(center, radius);
    }
}
