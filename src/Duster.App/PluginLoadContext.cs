using System.Reflection;
using System.Runtime.Loader;

namespace Duster.App;

public class PluginLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resovler;

    public PluginLoadContext(AssemblyDependencyResolver resolver)
    {
        _resovler = resolver;
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