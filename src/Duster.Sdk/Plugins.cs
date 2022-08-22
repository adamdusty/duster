using System.Reflection;
using System.Runtime.Loader;

namespace Duster.Sdk;

public class PluginService
{
    private List<string> _searchPaths;
    private List<PluginLoadContext> _loadContexts;
    private AssemblyLoadContext _context;

    public PluginService()
    {
        _context = new AssemblyLoadContext("ModContext", true);
        _loadContexts = new List<PluginLoadContext>();
        _searchPaths = new List<string>(
            new[]{
                @"/home/ad/dev/duster/build/App/mods/render-plugin",
                @"/home/ad/dev/duster/build/App/mods/test-plugin"
            }
        );

        _context.Resolving += SearchForLibrary;
    }

    private Assembly? SearchForLibrary(AssemblyLoadContext context, AssemblyName name)
    {
        foreach (var path in _searchPaths)
        {
            if (!string.IsNullOrEmpty(name.Name) && File.Exists(Path.Combine(path, $"{name.Name}.dll")))
            {
                return context.LoadFromAssemblyPath(Path.Combine(path, $"{name.Name}.dll"));
            }
        }

        return null;
    }

    public Assembly? LoadPluginAssemblyFromPath(string path)
    {
        var context = new PluginLoadContext(path);
        try
        {
            return _context.LoadFromAssemblyPath(path);
            // _loadContexts.Add(context);
            // return context.LoadFromAssemblyPath(path);
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
        if (!string.IsNullOrEmpty(assemblyPath) && File.Exists(assemblyPath))
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