using System.Text.Json;

namespace RayTracer.Library.Serialization.Serializers;

public abstract class ProxySerializer<TInstance, TFrom, TTo> : SerializerBase<TInstance, TFrom>
    where TInstance : ProxySerializer<TInstance, TFrom, TTo>, new()
{
    protected abstract ISerializer<TTo> ProxyTypeSerializer { get; }

    protected abstract TTo? Convert(TFrom? value);

    protected abstract TFrom? Convert(TTo? value);

    public override void Serialize(Utf8JsonWriter writer, TFrom? value)
    {
        TTo? proxy = Convert(value);
        ProxyTypeSerializer.Serialize(writer, proxy);
    }

    public override TFrom? Deserialize(ref Utf8JsonReader reader)
    {
        TTo? proxy = ProxyTypeSerializer.Deserialize(ref reader);
        return Convert(proxy);
    }
}
