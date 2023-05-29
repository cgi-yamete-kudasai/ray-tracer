using System;
using System.Collections.Generic;

namespace RayTracer.DependencyInjection;

public partial class ServiceContainer
{
    public static Builder CreateBuilder() => new();

    public class Builder
    {
        private readonly Dictionary<Type, ServiceDescriptor> _descriptors = new();

        public Builder AddSingleton<T>()
            where T : class, new()
        {
            _descriptors.Add(typeof(T), new()
            {
                Type = typeof(T),
                Lifetime = ServiceLifetime.Singleton
            });

            return this;
        }

        public Builder AddSingleton<T>(Func<T> factory)
            where T : class
        {
            _descriptors.Add(typeof(T), new()
            {
                Type = typeof(T),
                Lifetime = ServiceLifetime.Singleton,
                Factory = factory
            });

            return this;
        }

        public Builder AddTransient<T>()
            where T : class, new()
        {
            _descriptors.Add(typeof(T), new()
            {
                Type = typeof(T),
                Lifetime = ServiceLifetime.Transient
            });

            return this;
        }

        public ServiceContainer Build() => new(_descriptors);
    }
}
