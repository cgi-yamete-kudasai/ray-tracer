using System;
using System.Text.Json;
using RayTracer.Library.Serialization;

namespace RayTracer.Library.Extensions;

public static class Utf8JsonReaderExtensions
{
    public static void EnsureToken(ref this Utf8JsonReader reader, JsonTokenType token)
    {
        if (reader.TokenType != token)
            throw new SerializationException($"The JSON token was invalid: expected {token}, got {reader.TokenType}");
    }

    public static void EnsureTokenAndRead(ref this Utf8JsonReader reader, JsonTokenType token)
    {
        EnsureToken(ref reader, token);
        reader.Read();
    }

    public static void EnsureProperty(ref this Utf8JsonReader reader, ReadOnlySpan<byte> property)
    {
        EnsureToken(ref reader, JsonTokenType.PropertyName);

        if (!reader.ValueSpan.SequenceEqual(property))
            throw new SerializationException($"The JSON property name was invalid: expected {property.GetString()}, got {reader.ValueSpan.GetString()}");
    }

    public static void EnsurePropertyAndRead(ref this Utf8JsonReader reader, ReadOnlySpan<byte> property)
    {
        EnsureProperty(ref reader, property);
        reader.Read();
    }

    public static bool TryReadNull(ref this Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.Null)
            return false;

        reader.Read();
        return true;
    }
}
