using System.Reflection;
using System.Runtime.Loader;

namespace Duster.Sdk;

public static class PluginService
{
    public static List<IPlugin> LoadPluginsFromAssembly(string path)
    {
        var context = new AssemblyLoadContext("LoadContext", true);
        var assembly = context.LoadFromAssemblyPath(path);

        List<IPlugin> plugins = assembly.GetTypes()
            .Where(t => typeof(IPlugin).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as IPlugin)
            .Where(p => p is not null)
            .ToList()!;

        return plugins;
    }
}