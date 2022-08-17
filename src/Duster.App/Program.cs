using DefaultEcs;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;

        var loader = new PluginLoader(Path.Combine(exeDir, "plugins"));
        var plugins = loader.LoadAllPlugins();

        var world = new World();

        foreach (var p in plugins)
        {
            p.Initialize(world);
        }
    }
}
