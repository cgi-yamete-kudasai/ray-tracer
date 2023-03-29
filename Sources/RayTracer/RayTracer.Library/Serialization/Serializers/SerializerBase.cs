using System.Text.Json;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public abstract class SerializerBase<TInstance, T> : Singleton<TInstance>, ISerializer<T>
    where TInstance : Singleton<TInstance>, new()
{
    public abstract T? Deserialize(ref Utf8JsonReader reader);

    public abstract void Serialize(Utf8JsonWriter writer, T? value);
}
