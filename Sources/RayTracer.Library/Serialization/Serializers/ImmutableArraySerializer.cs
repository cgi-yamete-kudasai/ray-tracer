using System.Collections.Immutable;
using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public class ImmutableArraySerializer<T> : Singleton<ImmutableArraySerializer<T>>, ISerializer<ImmutableArray<T>>
{
    private readonly ISerializer<T> _itemSerializer;

    public ImmutableArraySerializer()
    {
        if (!SerializationHelper.TryGetSerializer(out _itemSerializer!))
            throw new SerializationException($"Can't find a custom serializer for {typeof(T)}");
    }

    public void Serialize(Utf8JsonWriter writer, ImmutableArray<T> value)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            _itemSerializer.Serialize(writer, item);
        }

        writer.WriteEndArray();
    }

    public ImmutableArray<T> Deserialize(ref Utf8JsonReader reader)
    {
        var builder = ImmutableArray.CreateBuilder<T>();

        reader.EnsureTokenAndRead(JsonTokenType.StartArray);

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            T item = _itemSerializer.Deserialize(ref reader)!;
            builder.Add(item);
        }

        reader.EnsureTokenAndRead(JsonTokenType.EndArray);

        return builder.ToImmutable();
    }
}
