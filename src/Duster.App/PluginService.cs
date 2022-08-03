using System.Reflection;
using System.Runtime.Loader;

namespace Duster.App;

public class PluginService
{
    private AssemblyLoadContext _loadContext;

    public PluginService()
    {
        _loadContext = new AssemblyLoadContext("LoadContext", true);
    }

    public Assembly LoadPlugin(string path)
    {
        return _loadContext.LoadFromAssemblyPath(path);
    }
}