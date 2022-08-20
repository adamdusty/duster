using System.Text.Json;
using Duster.Sdk;

namespace Duster.App;

public static class PluginLoading
{
    /// <summary>
    /// Searches all subdirectories of pluginDirectory for manifest files to load plugins.
    /// </summary>
    /// <param name="pluginDirectoryPath"></param>
    /// <returns>All types that implement IPlugin.</returns>
    public static List<IPlugin> LoadAllPluginsFromDirectory(string pluginDirectoryPath)
    {
        var pluginDirectoryInfo = new DirectoryInfo(pluginDirectoryPath);
        var plugins = new List<IPlugin>();
        foreach (var dir in pluginDirectoryInfo.EnumerateDirectories())
        {
            var directoryPlugins = LoadPluginsFromDirectory(dir.FullName);
            if (directoryPlugins is not null)
                plugins.AddRange(directoryPlugins);
        }

        return plugins;
    }

    /// <summary>
    /// Loads plugin types from the assembly paths defined in manifest.json in the directory at path.
    /// </summary>
    /// <param name="path">Path to the directory of the plugin.</param>
    /// <returns>All types in assembly that implement IPlugin.</returns>
    public static List<IPlugin>? LoadPluginsFromDirectory(string path)
    {

        var pluginDirectory = new DirectoryInfo(path);
        using var jsonStream =
            new FileStream(Path.Combine(pluginDirectory.FullName, "manifest.json"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        if (jsonStream is null)
            return null;

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var manifest = JsonSerializer.Deserialize<PluginManifest>(jsonStream, jsonOptions);
        if (manifest is null)
            return null;

        return manifest.AssemblyPaths.Select(p => Path.Combine(pluginDirectory.FullName, p)).SelectMany(p => Plugins.LoadTypesFromAssembly(p)).ToList();
    }
}