using System.Reflection;
using System.Runtime.Loader;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var pluginService = new PluginService();

        string[] pluginsPaths = new string[]{
            @"/home/ad/dev/duster/build/bin/Debug/Duster.TestPlugin/net6.0/Duster.TestPlugin.dll",
            @"/home/ad/dev/duster/build/bin/Debug/Duster.TestPlugin2/net6.0/Duster.TestPlugin2.dll",
        };

        var assemblies = pluginsPaths.Select(p => pluginService.LoadPlugin(p));

        var plugins = new List<IPlugin?>();
        foreach (var p in assemblies)
        {
            foreach (var t in p.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(t))
                {
                    var plugin = Activator.CreateInstance(t) as IPlugin;
                    if (plugin is not null)
                        plugins.Add(plugin);
                }
            }
        }

        foreach (var p in plugins)
        {
            p?.Load();
        }

        foreach (var p in plugins)
        {
            p?.Unload();
        }
    }
}
