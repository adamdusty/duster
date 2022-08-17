using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var loader = new PluginLoader(@"/home/ad/dev/duster/build/Debug/Duster.App/net6.0/plugins");
        var plugins = loader.LoadAllPlugins();

        foreach (var p in plugins)
        {
            System.Console.WriteLine(p.Name);
        }
    }
}
