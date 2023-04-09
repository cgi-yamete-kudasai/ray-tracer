using System;
using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Serialization.Serializers;

public class PlaneSerializer : SerializerBase<PlaneSerializer, Plane>
{
    public override void Serialize(Utf8JsonWriter writer, Plane? value)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(value.BasePoint));
        Vector3Serializer.Instance.Serialize(writer, value.BasePoint);

        writer.WritePropertyName(nameof(value.Normal));
        Vector3Serializer.Instance.Serialize(writer, value.Normal);

        writer.WriteEndObject();
    }

    public override Plane? Deserialize(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
        {
            return null;
        }

        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead("BasePoint"u8);
        Vector3 basePoint = Vector3Serializer.Instance.Deserialize(ref reader);

        reader.EnsurePropertyAndRead("Normal"u8);
        Vector3 normal = Vector3Serializer.Instance.Deserialize(ref reader);

        reader.EnsureTokenAndRead(JsonTokenType.EndObject);

        return new Plane(basePoint, normal);
    }
}