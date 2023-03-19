using System;

namespace RayTracer.IO.CLI;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ArgSwitchAttribute : Attribute
{
    public string Switch { get; }

    public ArgSwitchAttribute(string @switch)
    {
        Switch = @switch;
    }
}
