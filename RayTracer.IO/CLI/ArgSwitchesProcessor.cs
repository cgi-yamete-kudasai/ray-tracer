using System;
using System.Diagnostics;
using System.Reflection;
using RayTracer.Library.Utils;

namespace RayTracer.IO.CLI;

public class ArgSwitchesProcessor
{
    private readonly ArgSwitchesCollector _collector;

    public ArgSwitchesProcessor(AppDomain? domain = null)
    {
        domain ??= AppDomain.CurrentDomain;
        _collector = new(domain);
    }

    public void Process(string[] args)
    {
        var switchesMap = _collector.Collect();

        foreach (var arg in args)
        {
            string[] split = arg.Split('=');
            
            string name = split[0];
            string value = split[1];

            if (switchesMap.TryGetValue(name, out var switches))
            {
                foreach (var @switch in switches)
                    InvokeArgSwitch(@switch, value);
            }
        }
    }

    private static unsafe void InvokeArgSwitch(MethodInfo method, string value)
    {
        Type decl = method.DeclaringType!;

        if (decl.BaseType != typeof(Singleton<>).MakeGenericType(decl))
            throw new InvalidOperationException($"Methods with {nameof(ArgSwitchAttribute)} can only be singletons");

        MethodInfo instanceGetter = decl.BaseType?.GetTypeInfo().GetDeclaredMethod("get_Instance") ?? throw new UnreachableException();

        var ptr = (delegate*<object>)instanceGetter.MethodHandle.GetFunctionPointer();
        var instance = ptr();

        method.Invoke(instance, new object[] { value });
    }
}
