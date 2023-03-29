using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using RayTracer.Library.Reflection;

namespace RayTracer.Library.CLI;

public class ArgSwitchesIndexer : ModuleTypesIndexerBase<ArgSwitchesIndexer, ModuleArgSwitchesAttribute>
{
    public IReadOnlyDictionary<string, IReadOnlyList<MethodInfo>> Switches => _switches;

    private readonly Dictionary<string, IReadOnlyList<MethodInfo>> _switches = new();

    protected override void Process(Type type)
    {
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            foreach (var @switch in method.GetCustomAttributes<ArgSwitchAttribute>())
            {
                ref var entry = ref CollectionsMarshal.GetValueRefOrAddDefault(_switches, @switch.Switch, out _);
                entry ??= new List<MethodInfo>();
                ((List<MethodInfo>)entry).Add(method);
            }
        }
    }
}
