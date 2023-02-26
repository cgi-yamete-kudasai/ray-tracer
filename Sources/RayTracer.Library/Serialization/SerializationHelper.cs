using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using RayTracer.Library.Extensions;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Serialization;

public static class SerializationHelper
{
    private static ReadOnlySpan<byte> TypeProperty => "$Type"u8;

    private static ReadOnlySpan<byte> ValueProperty => "$Value"u8;

    private static readonly Dictionary<Type, ISerializer> _builtInTypeMap = new()
    {
        // to be added later on...
    };

    public static void Serialize<T>(Stream stream, T? value)
        where T : ISerializable<T>
    {
        JsonWriterOptions options = new()
        {
            Indented = true
        };

        using Utf8JsonWriter writer = new(stream, options);
        T.Serializer.Serialize(writer, value);
    }

    public static T? Deserialize<T>(Stream stream)
        where T : ISerializable<T>
    {
        MemoryStream ms = new();
        stream.CopyTo(ms);
        ReadOnlySpan<byte> bytes = ms.GetBuffer();

        Utf8JsonReader reader = new(bytes);
        reader.Read();
        return T.Serializer.Deserialize(ref reader);
    }

    public static void SerializePolymorphic(Utf8JsonWriter writer, object? value)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        Type type = value.GetType();

        writer.WriteStartObject();

        writer.WritePropertyName(TypeProperty);
        writer.WriteStringValue(type.FullName);

        if (!TryGetSerializer(type, out var serializer))
            throw new SerializationException($"Can't serialize the value of type {type}: serializer not found");

        writer.WritePropertyName(ValueProperty);
        serializer.Serialize(writer, value);

        writer.WriteEndObject();
    }

    public static object? DeserializePolymorphic(ref Utf8JsonReader reader)
    {
        if (reader.TryReadNull())
            return null;

        reader.EnsureTokenAndRead(JsonTokenType.StartObject);

        reader.EnsurePropertyAndRead(TypeProperty);
        string typeString = reader.GetString()!;
        reader.Read();

        Type type = Type.GetType(typeString) ?? throw new SerializationException($"Can't find the type from the string \"{typeString}\"");

        if (!TryGetSerializer(type, out var serializer))
            throw new SerializationException($"Can't deserialize the value of type {type}: serializer not found");

        reader.EnsurePropertyAndRead(ValueProperty);
        object? value = serializer.Deserialize(ref reader);
        reader.EnsureTokenAndRead(JsonTokenType.EndObject);

        return value;
    }

    public static bool TryGetSerializer<T>([MaybeNullWhen(false)] out ISerializer<T> serializer)
    {
        if (TryGetSerializer(typeof(T), out var instance))
        {
            serializer = (ISerializer<T>)instance;
            return true;
        }

        if (typeof(T).IsPolymorphic())
        {
            serializer = PolymorphicSerializer<T>.Instance;
            return true;
        }

        serializer = null;
        return false;
    }

    public static unsafe bool TryGetSerializer(Type type, [MaybeNullWhen(false)] out ISerializer serializer)
    {
        if (_builtInTypeMap.TryGetValue(type, out serializer))
            return true;

        Type serializableIface = typeof(ISerializable<>).MakeGenericType(type);

        if (!type.GetInterfaces().Contains(serializableIface))
        {
            serializer = null;
            return false;
        }

        InterfaceMapping map = type.GetInterfaceMap(serializableIface);
        MethodInfo? getter = null;

        for(int i = 0; i < map.InterfaceMethods.Length; i++)
        {
            MethodInfo method = map.InterfaceMethods[i];

            if (method.Name == $"get_{nameof(ISerializable<int>.Serializer)}")
            {
                getter = map.TargetMethods[i];
                break;
            }
        }

        if (getter is null)
            throw new UnreachableException();

        var ptr = (delegate*<ISerializer>)getter.MethodHandle.GetFunctionPointer();
        serializer = ptr();
        return true;
    }
}
