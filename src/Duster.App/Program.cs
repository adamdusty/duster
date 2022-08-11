using System.Reflection;
using System.Runtime.Loader;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        var manifest = new PluginManifest();
        var collector = new DependencyCollector(@"./plugins/");
        var deps = collector.CollectDependencies(manifest);
    }
}
