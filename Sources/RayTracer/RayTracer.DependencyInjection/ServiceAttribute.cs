using System;

namespace RayTracer.DependencyInjection;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ServiceAttribute : Attribute { }
