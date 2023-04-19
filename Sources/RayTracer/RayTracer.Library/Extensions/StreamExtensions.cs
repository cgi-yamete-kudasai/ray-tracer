using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace RayTracer.Library.Extensions;

public static class StreamExtensions
{
    public static unsafe T NativeRead<T>(this Stream stream)
        where T : unmanaged
    {
        int size = Unsafe.SizeOf<T>();

        Unsafe.SkipInit(out T result);

        Span<byte> bytes = new(&result, size);

        int bytesRead = stream.Read(bytes);

        if (bytesRead < size)
            throw new IOException($"Unable to read {typeof(T)}: fewer bytes read than requested ({bytesRead}/{size}).");

        return result;
    }

    public static unsafe void NativeWrite<T>(this Stream stream, T value)
        where T : unmanaged
    {
        int size = Unsafe.SizeOf<T>();
        Span<byte> bytes = new(&value, size);
        stream.Write(bytes);
    }
}
