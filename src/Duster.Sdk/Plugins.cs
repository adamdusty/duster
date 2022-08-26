using System.Reflection;
using System.Runtime.Loader;

namespace Duster.Sdk;

public class PluginService
{
    private List<string> _searchPaths;
    private AssemblyLoadContext _context;

    public PluginService(IEnumerable<string> searchPaths)
    {
        _context = new AssemblyLoadContext("ModContext", true);
        _context = AssemblyLoadContext.Default;
        _searchPaths = new List<string>(searchPaths);

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
        try
        {
            return _context.LoadFromAssemblyPath(path);
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
}