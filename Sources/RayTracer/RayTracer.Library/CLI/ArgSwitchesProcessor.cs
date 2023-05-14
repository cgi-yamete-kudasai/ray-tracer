using System;
using System.Collections.Generic;
using RayTracer.DependencyInjection;

namespace RayTracer.Library.CLI;

public class ArgSwitchesProcessor
{
    [Service]
    private readonly ArgSwitchesIndexer _switchesIndexer = null!;

    public void Process(string[] args, IArgSwitchesProvider provider)
    {
        var switchesMap = _switchesIndexer.Switches;

        var listenersMap = new Dictionary<Type, object>();

        foreach (var listener in provider.Listeners)
            listenersMap.Add(listener.GetType(), listener);

        foreach (var arg in args)
        {
            string[] split = arg.Split('=');
            
            string name = split[0];
            string value = split[1];

            if (switchesMap.TryGetValue(name, out var switches))
            {
                foreach (var @switch in switches)
                {
                    if (listenersMap.TryGetValue(@switch.DeclaringType!, out var listener))
                        @switch.Invoke(listener, new object[] { value });
                }
            }
        }
    }
}
