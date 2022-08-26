using System.Text.Json;
using System.Reflection;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoader;

public class ModLoader
{
    private readonly IFileSystem _fileSystem;
    private string[] _searchPaths;

    public ModLoader(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public ModLoader() : this(fileSystem: new FileSystem()) { }

    public async Task<List<string>> GetAssemblyPaths(string modDirectoryPath)
    {
        if (!Directory.Exists(modDirectoryPath))
            return new List<string>();

        var files = Directory.EnumerateDirectories(modDirectoryPath)
            .Select(d => Path.Combine(d, "manifest.json"))
            .Where(p => File.Exists(p));

        // This is ugly but I can't currently come up with another way 
        // to keep the directory path with the manifest object
        var manifests = await Task.WhenAll(files.Select(async f => (Path.GetDirectoryName(f), await ReadManifest(f))));
        return manifests.Where(m => m.Item1 is not null && m.Item2 is not null)
            .Select(m => Path.Combine(m.Item1!, m.Item2!.AssemblyPath))
            .ToList();
    }

    public async Task<List<Assembly>> LoadAssemblies(IEnumerable<string> assemblyPaths)
    {

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

    // /// <summary>
    // /// Loads mod from path to directory.
    // /// </summary>
    // /// <param name="path">Directory of the mod to be loaded.</param>
    // /// <returns>Assembly of the mod. Null if any errors occur during loading.</returns>
    // public async Task<Assembly?> LoadModAssembly(string path)
    // {
    //     // Read manifest file in directory at path
    //     var manifest = await ReadManifest(Path.Combine(path, "manifest.json"));
    //     if (manifest is null)
    //         return null;

    //     // Load assembly based on path in manifest
    //     return _pluginService.LoadPluginAssemblyFromPath(Path.Combine(path, manifest.AssemblyPath));
    // }

    // // Method to load mod assembly if you've already read the manifest.
    // // Not sure I like this.
    // // REVIEW
    // private Assembly? LoadModAssembly(string path, Manifest manifest)
    // {
    //     return _pluginService.LoadPluginAssemblyFromPath(Path.Combine(path, manifest.AssemblyPath));
    // }

    // public async Task<ModInfo?> LoadMod(string path)
    // {
    //     var manifest = await ReadManifest(Path.Combine(path, "manifest.json"));
    //     if (manifest is null)
    //         return null;

    //     var assembly = LoadModAssembly(path, manifest);
    //     if (assembly is null)
    //         return null;

    //     return new ModInfo(assembly, path, manifest.ModName);
    // }
}