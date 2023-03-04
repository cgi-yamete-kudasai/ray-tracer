using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Serialization.Serializers;

public class Vector3Serializer : SerializerBase<Vector3Serializer, Vector3>
{
    public override void Serialize(Utf8JsonWriter writer, Vector3 value)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(Vector3.X));
        writer.WriteNumberValue(value.X);

        writer.WritePropertyName(nameof(Vector3.Y));
        writer.WriteNumberValue(value.Y);

        writer.WritePropertyName(nameof(Vector3.Z));
        writer.WriteNumberValue(value.Z);

        writer.WriteEndObject();
    }

    public override Vector3 Deserialize(ref Utf8JsonReader reader)
    {
        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead("X"u8);
        reader.EnsureToken(JsonTokenType.Number);
        float x = reader.GetSingle();
        reader.Read();

        reader.EnsurePropertyAndRead("Y"u8);
        reader.EnsureToken(JsonTokenType.Number);
        float y = reader.GetSingle();
        reader.Read();

        reader.EnsurePropertyAndRead("Z"u8);
        reader.EnsureToken(JsonTokenType.Number);
        float z = reader.GetSingle();
        reader.Read();

        reader.EnsureTokenAndRead(JsonTokenType.EndObject);

        return new(x, y, z);
    }
}
