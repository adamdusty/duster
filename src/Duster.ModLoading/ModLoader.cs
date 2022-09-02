using System.Text.Json;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModLoader
{
    private IFileSystem _fileSystem;

    public ModLoader(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }
    public ModLoader() : this(new FileSystem()) { }


    /// <summary>
    /// If directory <paramref name="dir" /> exists, find all directories within that directory
    /// that contain a manifest.json file. Read the manifest files and return a list of <see cref="ModInfo" />
    /// for each mod folder. If there are no directories with manifest files, return null.
    /// </summary>
    /// <param name="dir">Directory of mods.</param>
    /// <returns>List of <see cref="ModInfo" /> or null</returns>
    public async Task<IEnumerable<ModInfo>?> GetInformationForAllMods(string dir)
    {
        if (!_fileSystem.Directory.Exists(dir))
            return null;

        var files = _fileSystem.Directory.EnumerateDirectories(dir)
            .Select(d => _fileSystem.Path.Combine(d, "manifest.json"))
            .Where(p => _fileSystem.File.Exists(p));

        if (files?.Any() != true)
            return null;

        var info = new ConcurrentBag<ModInfo>();

        // This is hella ugly but I can't currently come up with another way 
        // to keep the directory path with the manifest object without using a foreach
        await Task.WhenAll(files.Select(async f =>
        {
            var manifest = await ReadManifest(f);
            if (manifest is null)
                return;

            info.Add(new ModInfo(
                manifest,
                _fileSystem.Path.GetDirectoryName(f)
            ));
        }));

        return info;
    }

    /// <summary>
    /// Looks for assembly at the assembly path from <see cref="ModInfo"/>. If it exists and is a .dll,
    /// tries to load the assembly into <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Load context for mods.</param>
    /// <param name="info">Mod information.</param>
    /// <returns><see cref="Mod"/> or null isn't a .dll or doesn't exist.</returns>
    public Mod? LoadMod(AssemblyLoadContext context, ModInfo info)
    {
        var path = _fileSystem.Path.Combine(info.Directory, info.Manifest.AssemblyPath);
        if (!_fileSystem.File.Exists(path) || _fileSystem.Path.GetExtension(path) != ".dll")
            return null;

        var assembly = context.LoadFromAssemblyPath(path);
        return new Mod(info.Manifest, assembly);
    }

    public List<ISystemProvider> InstanceModSystemProviders(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as ISystemProvider)
            .Where(sp => sp is not null)
            .ToList()!;
    }

    /// <summary>
    /// Load manifest file.
    /// </summary>
    /// <param name="path">Path to manifest file.</param>
    /// <returns>Manifest object if successfully read, otherwise null.</returns>
    private async Task<Manifest?> ReadManifest(string path)
    {
        try
        {
            using var json = _fileSystem.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await JsonSerializer.DeserializeAsync<Manifest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            return null;
        }
    }

}