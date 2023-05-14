using System;
using System.Collections.Generic;
using System.Reflection;

namespace RayTracer.DependencyInjection;

public partial class ServiceContainer : IDisposable
{
    private readonly Dictionary<Type, object>  _services = new();

    private readonly IReadOnlyDictionary<Type, ServiceDescriptor> _descriptors;

    private ServiceContainer(IReadOnlyDictionary<Type, ServiceDescriptor> descriptors)
    {
        _descriptors = descriptors;
        RegisterSingletons();
    }

    public T ResolveService<T>()
        where T : class
        => (T)ResolveService(typeof(T));

    private void RegisterSingletons()
    {
        foreach (var (type, descriptor) in _descriptors)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
                ResolveService(type);
        }
    }

    private object ResolveService(Type type)
    {
        if (!_descriptors.TryGetValue(type, out var descriptor))
            throw new KeyNotFoundException($"Can't resolve service: {type}");

        if (descriptor.Lifetime == ServiceLifetime.Singleton &&  _services.TryGetValue(type, out var value))
            return value;

        object instance = descriptor.Factory?.Invoke() ?? Activator.CreateInstance(type)!;

        foreach (var member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (member.GetCustomAttribute<ServiceAttribute>() is null)
                continue;

            if (member is FieldInfo field)
            {
                Type serviceType = field.FieldType;
                object service = ResolveService(serviceType);
                field.SetValue(instance, service);
            }

            if (member is PropertyInfo property)
            {
                Type serviceType = property.PropertyType;
                object service = ResolveService(serviceType);
                property.SetValue(instance, service);
            }
        }

        _services.Add(type, instance);

        return instance;
    }

    public void Dispose()
    {
        foreach (var instance in _services.Values)
        {
            if (instance is IDisposable disposable)
                disposable.Dispose();
        }

        _services.Clear();
    }
}
