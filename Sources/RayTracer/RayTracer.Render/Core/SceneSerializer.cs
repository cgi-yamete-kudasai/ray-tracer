using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Serialization.Serializers;
using RayTracer.Library.Shapes;
using RayTracer.Render.Lights;

namespace RayTracer.Render.Core;

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
        ImmutableArraySerializer<IIntersectable>.Instance.Serialize(writer, value.Shapes);

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
        var shapes = ImmutableArraySerializer<IIntersectable>.Instance.Deserialize(ref reader)!;

        reader.EnsurePropertyAndRead("Lights"u8);
        var lights = ImmutableArraySerializer<ILight>.Instance.Deserialize(ref reader);

        reader.EnsureTokenAndRead(JsonTokenType.EndObject);

        return new(shapes, lights);
    }
}
