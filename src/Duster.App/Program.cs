using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var service = new PluginService();
        var plugins = service.LoadPluginsFromAssembly(@"/home/ad/dev/duster/build/bin/Debug/Duster.TestPlugin/net6.0/Duster.TestPlugin.dll");

        foreach (var plugin in plugins)
        {
            System.Console.WriteLine(plugin.Name);
        }
    }
}
