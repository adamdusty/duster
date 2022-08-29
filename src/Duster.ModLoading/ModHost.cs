using System.Reflection;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModHost
{
    private ModLoader _loader;
    private Dictionary<ModInfo, Assembly> _mods;
    public IReadOnlyDictionary<ModInfo, Assembly> Mods => _mods;

    public ModHost(IFileSystem fs)
    {
        _loader = new ModLoader(fs);
        _mods = new Dictionary<ModInfo, Assembly>();
    }

    public ModHost() : this(new FileSystem()) { }

    public void Enable(string modId) { throw new NotImplementedException(); }
    public void Disable(string modId) { throw new NotImplementedException(); }
    public async Task<IReadOnlyDictionary<ModInfo, Assembly>> LoadModsFromDirectory(string dir)
    {
        var infos = await _loader.GetInformationForAllMods(dir);
        if (infos is null)
            return Mods;

        _mods = _loader.LoadModAssemblies(infos);
        return Mods;
    }

    public List<ISystemProvider>? InstanceModSystemProviders(IEnumerable<Assembly> assemblies)
        => _loader.InstanceModTypes<ISystemProvider>(assemblies);
}