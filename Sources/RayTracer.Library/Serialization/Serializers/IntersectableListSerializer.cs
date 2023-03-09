using System;
using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.Serialization.Serializers;

public class IntersectableListSerializer : SerializerBase<IntersectableListSerializer, IntersectableList>
{
    private readonly ISerializer<IIntersectable> _itemSerializer;

    public IntersectableListSerializer()
    {
        _itemSerializer = new PolymorphicSerializer<IIntersectable>();
    }

    public override void Serialize(Utf8JsonWriter writer, IntersectableList? value)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (var item in value)
        {
            _itemSerializer.Serialize(writer, item);
        }

        writer.WriteEndArray();
    }

    public override IntersectableList? Deserialize(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
        {
            return null;
        }
        
        IntersectableList intersectableList = new IntersectableList();

        reader.EnsureTokenAndRead(JsonTokenType.StartArray);

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            IIntersectable item = _itemSerializer.Deserialize(ref reader)!;
            intersectableList.Add(item);
        }
        
        reader.EnsureTokenAndRead(JsonTokenType.EndArray);

        return intersectableList;
    }
}