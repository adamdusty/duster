using System.Reflection;

namespace Duster.App;

public class PluginService
{
    public Assembly LoadPlugin(string path)
    {
        PluginLoadContext context = new PluginLoadContext(path);
        return context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
    }
}