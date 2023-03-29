using System;
using System.Text;

namespace RayTracer.Library.Extensions;

public static class SpanExtensions
{
    public static string GetString(this ReadOnlySpan<byte> span, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(span);
    }
}
