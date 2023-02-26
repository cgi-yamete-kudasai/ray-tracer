using System.Text.Json;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public class PolymorphicSerializer<T> : Singleton<PolymorphicSerializer<T>>, ISerializer<T>
{
    public void Serialize(Utf8JsonWriter writer, T? value)
    {
        SerializationHelper.SerializePolymorphic(writer, value);
    }

    public T? Deserialize(ref Utf8JsonReader reader)
    {
        return (T?)SerializationHelper.DeserializePolymorphic(ref reader);
    }
}
