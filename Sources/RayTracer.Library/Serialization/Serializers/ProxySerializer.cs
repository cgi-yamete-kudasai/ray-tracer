using System.Text.Json;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Serialization.Serializers;

public abstract class ProxySerializer<TInstance, TFrom, TTo> : Singleton<TInstance>, ISerializer<TFrom>
    where TInstance : ProxySerializer<TInstance, TFrom, TTo>, new()
{
    protected abstract ISerializer<TTo> ProxyTypeSerializer { get; }

    protected abstract TTo? Convert(TFrom? value);

    protected abstract TFrom? Convert(TTo? value);

    public void Serialize(Utf8JsonWriter writer, TFrom? value)
    {
        TTo? proxy = Convert(value);
        ProxyTypeSerializer.Serialize(writer, proxy);
    }

    public TFrom? Deserialize(ref Utf8JsonReader reader)
    {
        TTo? proxy = ProxyTypeSerializer.Deserialize(ref reader);
        return Convert(proxy);
    }
}
