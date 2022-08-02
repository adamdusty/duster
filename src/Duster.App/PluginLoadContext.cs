using System.Reflection;
using System.Runtime.Loader;

namespace Duster.App;

public class PluginLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resovler;

    /// <summary>
    /// Creates instance with path to plugin assembly.
    /// </summary>
    /// <param name="path">Path to plugin assembly</param>
    public PluginLoadContext(string path)
    {
        _resovler = new AssemblyDependencyResolver(path);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resovler.ResolveAssemblyToPath(assemblyName);

        if (assemblyPath is not null)
            return LoadFromAssemblyPath(assemblyPath);

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libPath = _resovler.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libPath is not null)
            return LoadUnmanagedDllFromPath(libPath);

        return IntPtr.Zero;
    }
}