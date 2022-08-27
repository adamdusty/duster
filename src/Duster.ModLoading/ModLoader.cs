using System.Text.Json;
using System.Reflection;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModLoader
{
    private readonly PluginService _pluginService;
    private readonly IFileSystem _fileSystem;

    public ModLoader(IEnumerable<string> searchPaths, IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _pluginService = new PluginService(searchPaths);
    }

    public ModLoader(IEnumerable<string> searchPaths)
        : this(searchPaths: searchPaths, fileSystem: new FileSystem()) { }

    public async Task<List<string>> GetAssemblyPaths(string modDirectoryPath)
    {
        if (!_fileSystem.Directory.Exists(modDirectoryPath))
            return new List<string>();

        var files = _fileSystem.Directory.EnumerateDirectories(modDirectoryPath)
            .Select(d => _fileSystem.Path.Combine(d, "manifest.json"))
            .Where(p => _fileSystem.File.Exists(p));

        // This is ugly but I can't currently come up with another way 
        // to keep the directory path with the manifest object
        var manifests = await Task.WhenAll(files.Select(async f => (_fileSystem.Path.GetDirectoryName(f), await ReadManifest(f))));
        return manifests.Where(m => m.Item1 is not null && m.Item2 is not null)
            .Select(m => _fileSystem.Path.Combine(m.Item1!, m.Item2!.AssemblyPath))
            .ToList();
    }

    public List<Assembly> LoadAssemblies(IEnumerable<string> assemblyPaths)
    {
        return assemblyPaths
            .Select(p => _pluginService.LoadPluginAssemblyFromPath(p))
            .Where(a => a is not null)
            .ToList()!;
    }

    public List<ISystemProvider> InstanceSystemProviders(IEnumerable<Assembly> assemblies)
    {
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as ISystemProvider)
            .Where(isp => isp is not null)
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
            using var json = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await JsonSerializer.DeserializeAsync<Manifest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            return null;
        }
    }
}