using System.Reflection;
using System.Runtime.Loader;

namespace Duster.Sdk;

public class PluginService
{
    private List<PluginLoadContext> _loadContexts;

    public PluginService()
    {
        _loadContexts = new List<PluginLoadContext>();
    }

    public Assembly? LoadPluginAssemblyFromPath(string path)
    {
        var context = new PluginLoadContext(path);
        try
        {
            _loadContexts.Add(context);
            return context.LoadFromAssemblyPath(path);
        }
        catch (Exception e) when (
            e is ArgumentException ||
            e is ArgumentNullException ||
            e is FileLoadException ||
            e is FileNotFoundException ||
            e is BadImageFormatException
        )
        {
            return null;
        }
    }

    public List<IPlugin> InstantiateTypesFromPluginAssembly(Assembly assembly)
    {
        List<IPlugin> plugins = assembly.GetTypes()
            .Where(t => typeof(IPlugin).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as IPlugin)
            .Where(p => p is not null)
            .ToList()!;

        return plugins;
    }
}

public class PluginLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}