using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RayTracer.IO.CLI;

public class ArgSwitchesCollector
{
    private readonly AppDomain _domain;

    public ArgSwitchesCollector(AppDomain domain)
    {
        _domain = domain;
    }

    public IReadOnlyDictionary<string, IReadOnlyList<MethodInfo>> Collect()
    {
        Dictionary<string, IReadOnlyList<MethodInfo>> switches = new();

        foreach (var type in CollectArgSwitchTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var @switch in method.GetCustomAttributes<ArgSwitchAttribute>())
                {
                    ref var entry = ref CollectionsMarshal.GetValueRefOrAddDefault(switches, @switch.Switch, out _);
                    entry ??= new List<MethodInfo>();
                    ((List<MethodInfo>)entry).Add(method);
                }
            }
        }

        return switches;
    }

    private IReadOnlyList<Type> CollectArgSwitchTypes()
    {
        List<Type> types = new();

        foreach (var assembly in _domain.GetAssemblies())
        {
            foreach (var module in assembly.Modules)
            {
                if (module.GetCustomAttribute<ModuleArgSwitchesAttribute>() is not { } attr)
                    continue;

                types.AddRange(attr.Types);
            }
        }

        return types;
    }
}
