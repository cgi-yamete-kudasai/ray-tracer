using System;
using System.Diagnostics;
using System.Reflection;
using RayTracer.Library.Utils;

namespace RayTracer.Library.CLI;

public static class ArgSwitchesProcessor
{
    public static void Process(string[] args)
    {
        var switchesMap = ArgSwitchesIndexer.Instance.Switches;

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
