using System.Reflection;
using System.Runtime.Loader;
using System.IO.Abstractions;
using DefaultEcs;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModHost
{
    private IFileSystem _fileSystem;
    private List<string> _dependencySearchPaths;
    private ModLoader _loader;
    private Dictionary<string, Mod> _mods;
    private AssemblyLoadContext _loadContext;
    public IReadOnlyDictionary<string, Mod> Mods => _mods;

    public ModHost(IFileSystem fs)
    {
        _fileSystem = fs;
        _loader = new ModLoader(fs);
        _mods = new Dictionary<string, Mod>();
        _dependencySearchPaths = new List<string>();
        _loadContext = new AssemblyLoadContext("ModLoadContext", true);
        _loadContext.Resolving += SearchForLibrary;
    }

    public ModHost() : this(new FileSystem()) { }

    public void Enable(string modId) { throw new NotImplementedException(); }
    public void Disable(string modId) { throw new NotImplementedException(); }
    public async Task<IReadOnlyDictionary<string, Mod>> LoadModsFromDirectory(string dir)
    {
        _dependencySearchPaths = _fileSystem.Directory.GetDirectories(dir).ToList();

        var modInfos = await _loader.GetInformationForAllMods(dir);
        if (modInfos is null)
            return Mods;

        foreach (var info in modInfos)
        {
            var mod = _loader.LoadMod(_loadContext, info);
            if (mod is not null)
                _mods.Add(info.Manifest.ModName, mod);
        }

        foreach (var mod in Mods.Values)
        {
            var providers = _loader.InstanceModSystemProviders(mod.Assembly);
            if (providers is null)
                continue;
        }

        return Mods;
    }

    public List<SystemInfo> InitializeMods(World world)
    {
        return Mods.Values.Select(m => m.Assembly)
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as ISystemProvider)
            .Where(sp => sp is not null)
            .Select(sp => sp!.GetSystemInfo(world))
            .ToList();
    }

    private Assembly? SearchForLibrary(AssemblyLoadContext context, AssemblyName name)
    {
        foreach (var path in _dependencySearchPaths)
        {
            if (!string.IsNullOrEmpty(name.Name) && File.Exists(Path.Combine(path, $"{name.Name}.dll")))
            {
                return context.LoadFromAssemblyPath(Path.Combine(path, $"{name.Name}.dll"));
            }
        }

        return null;
    }
}