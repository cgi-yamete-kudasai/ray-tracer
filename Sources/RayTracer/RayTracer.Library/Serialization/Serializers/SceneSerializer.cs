using System.Collections.Immutable;
using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Lights;
using RayTracer.Library.Shapes;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public class SceneSerializer : SerializerBase<SceneSerializer, Scene>
{
    public override void Serialize(Utf8JsonWriter writer, Scene? value)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(Scene.Shapes));
        IntersectableListSerializer.Instance.Serialize(writer, value.Shapes);

        writer.WritePropertyName(nameof(Scene.Lights));
        ImmutableArraySerializer<ILight>.Instance.Serialize(writer, value.Lights);

        writer.WriteEndObject();
    }

    public override Scene? Deserialize(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
            return null;

        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead("Shapes"u8);
        var shapes = IntersectableListSerializer.Instance.Deserialize(ref reader)!;

        reader.EnsurePropertyAndRead("Lights"u8);
        var lights = ImmutableArraySerializer<ILight>.Instance.Deserialize(ref reader);

        reader.EnsureTokenAndRead(JsonTokenType.EndObject);

        return new(shapes, lights);
    }
}
