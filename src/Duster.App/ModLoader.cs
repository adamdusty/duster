using System.Text.Json;
using System.Reflection;
using Duster.Sdk;

namespace Duster.App;

public class ModLoader
{
    private PluginService _pluginService;

    public ModLoader()
    {
        _pluginService = new PluginService();
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
    public async Task<Assembly?> LoadMod(string path)
    {
        // Read manifest file in directory at path
        var manifest = await ReadManifest(Path.Combine(path, "manifest.json"));
        if (manifest is null)
            return null;

        // Load assembly based on path in manifest
        return _pluginService.LoadPluginAssemblyFromPath(Path.Combine(path, manifest.AssemblyPath));
    }

}