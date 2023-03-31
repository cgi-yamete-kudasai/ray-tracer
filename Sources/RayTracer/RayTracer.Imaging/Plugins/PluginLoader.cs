using System;
using System.IO;
using System.Reflection;

namespace RayTracer.Imaging.Plugins;

public class PluginLoader
{
    public const string DEFAULT_PLUGINS_PATH = @"..\..\..\..\..\..\Plugins";

    public string PluginsPath { get; }

    public PluginLoader(string? pluginsPath = null)
    {
        PluginsPath = pluginsPath ?? DEFAULT_PLUGINS_PATH;
    }

    public void LoadPlugins()
    {
        DirectoryInfo dir = new(PluginsPath);

        if (!dir.Exists)
            throw new InvalidOperationException($"Plugin directory {PluginsPath} not found");

        foreach (var file in dir.EnumerateFiles("*.dll", SearchOption.AllDirectories))
            Assembly.LoadFrom(file.FullName); // load assemblies into current domain
    }
}
