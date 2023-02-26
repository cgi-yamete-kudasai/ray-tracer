using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public class Vector3Serializer : Singleton<Vector3Serializer>, ISerializer<Vector3>
{
    public void Serialize(Utf8JsonWriter writer, Vector3 value)
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

    public Vector3 Deserialize(ref Utf8JsonReader reader)
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
