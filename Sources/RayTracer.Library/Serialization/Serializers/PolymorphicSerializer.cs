using System.Text.Json;

namespace RayTracer.Library.Serialization.Serializers;

public class PolymorphicSerializer<T> : SerializerBase<PolymorphicSerializer<T>, T>
{
    public override void Serialize(Utf8JsonWriter writer, T? value)
    {
        SerializationHelper.SerializePolymorphic(writer, value);
    }

    public override T? Deserialize(ref Utf8JsonReader reader)
    {
        return (T?)SerializationHelper.DeserializePolymorphic(ref reader);
    }
}
