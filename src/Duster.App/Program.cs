using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var loader = new PluginLoader(@"./plugins");
        var plugins = loader.LoadAllPlugins();

        foreach (var p in plugins)
        {
            System.Console.WriteLine(p.Name);
        }
    }
}
