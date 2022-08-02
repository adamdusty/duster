using System.Reflection;
using System.Runtime.Loader;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var pluginService = new PluginService();

        var pluginAssembly = pluginService.LoadPlugin("");
    }
}
