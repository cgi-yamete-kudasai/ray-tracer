using System;

namespace RayTracer.DependencyInjection;

internal readonly struct ServiceDescriptor
{
    public required Type Type { get; init; }
    
    public required ServiceLifetime Lifetime { get; init; }

    public Func<object>? Factory { get; init; }
}
