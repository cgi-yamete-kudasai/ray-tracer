using System.Text.Json;

namespace RayTracer.Library.Serialization;

public interface ISerializer
{
    void Serialize(Utf8JsonWriter writer, object? value);

    object? Deserialize(ref Utf8JsonReader reader);
}

public interface ISerializer<T> : ISerializer
{
    void Serialize(Utf8JsonWriter writer, T? value);

    new T? Deserialize(ref Utf8JsonReader reader);

    void ISerializer.Serialize(Utf8JsonWriter writer, object? value) => Serialize(writer, (T?)value);

    object? ISerializer.Deserialize(ref Utf8JsonReader reader) => Deserialize(ref reader);
}
