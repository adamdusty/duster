using System.Text.Json;
using System.Reflection;
using Duster.Sdk;

namespace Duster.ModLoader;

public class ModLoader
{
    private PluginService _pluginService;

    public ModLoader(IEnumerable<string> searchPaths)
    {
        _pluginService = new PluginService(searchPaths);
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

    /// <summary>
    /// Loads mod from path to directory.
    /// </summary>
    /// <param name="path">Directory of the mod to be loaded.</param>
    /// <returns>Assembly of the mod. Null if any errors occur during loading.</returns>
    public async Task<Assembly?> LoadModAssembly(string path)
    {
        // Read manifest file in directory at path
        var manifest = await ReadManifest(Path.Combine(path, "manifest.json"));
        if (manifest is null)
            return null;

        // Load assembly based on path in manifest
        return _pluginService.LoadPluginAssemblyFromPath(Path.Combine(path, manifest.AssemblyPath));
    }

    // Method to load mod assembly if you've already read the manifest.
    // Not sure I like this.
    // REVIEW
    private Assembly? LoadModAssembly(string path, Manifest manifest)
    {
        return _pluginService.LoadPluginAssemblyFromPath(Path.Combine(path, manifest.AssemblyPath));
    }

    public async Task<ModInfo?> LoadMod(string path)
    {
        var manifest = await ReadManifest(Path.Combine(path, "manifest.json"));
        if (manifest is null)
            return null;

        System.Console.WriteLine($"Manifest: {manifest.AssemblyPath}");

        var assembly = LoadModAssembly(path, manifest);
        if (assembly is null)
            return null;

        return new ModInfo(assembly, path, manifest.ModName);
    }
}