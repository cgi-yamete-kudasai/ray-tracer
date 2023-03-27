using System;

namespace RayTracer.Library.Extensions;

public static class TypeExtensions
{
    public static bool IsPolymorphic(this Type type)
    {
        return type.IsInterface || type.IsAbstract || type == typeof(object);
    }
}
