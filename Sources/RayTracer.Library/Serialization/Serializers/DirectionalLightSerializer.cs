using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Lights;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Serialization.Serializers;

public class DirectionalLightSerializer : SerializerBase<DirectionalLightSerializer, DirectionalLight>
{
    public override void Serialize(Utf8JsonWriter writer, DirectionalLight? value)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(DirectionalLight.Color));
        ColorRGBSerializer.Instance.Serialize(writer, value.Color);

        writer.WritePropertyName(nameof(DirectionalLight.Direction));
        Vector3Serializer.Instance.Serialize(writer, value.Direction);

        writer.WriteEndObject();
    }

    public override DirectionalLight? Deserialize(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
            return null;

        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead("Color"u8);
        ColorRGB color = ColorRGBSerializer.Instance.Deserialize(ref reader);

        reader.EnsurePropertyAndRead("Direction"u8);
        Vector3 direction = Vector3Serializer.Instance.Deserialize(ref reader);

        reader.Read();

        return new(direction, color);
    }
}
