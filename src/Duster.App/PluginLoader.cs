using System.Threading.Tasks;
using System.Text.Json;
using Duster.Sdk;

namespace Duster.App;

public static class PluginLoading
{
    /// <summary>
    /// If path is a directory, enumerates directories at path, and calls <see cref="LoadPluginFromDirectory"/>
    /// on each directory.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<IPlugin>? LoadPluginsFromDirectory(string path)
    {
        // Iterate through directory folders
        string pluginDirectory = Path.GetFullPath(path);
        if (!Directory.Exists(pluginDirectory))
            return null;

        var plugins = new List<IPlugin>();
        var pluginDirectories = Directory.GetDirectories(pluginDirectory);
        Parallel.ForEach(pluginDirectories, (s) =>
        {

        });
        foreach (var dir in Directory.EnumerateDirectories(pluginDirectory))
        {
            var directoryPlugins = PluginLoading.LoadPluginFromDirectory(dir);
            if (directoryPlugins is not null)
            {
                plugins.AddRange(directoryPlugins);
            }
        }

        return plugins;
    }

    /// <summary>
    /// Loads plugin from directory path.
    /// Looks for manifest.json file to get the path to the plugin assembly.
    /// Attempts to load the assembly.
    /// Loads all plugin implementing types from the assembly.
    /// </summary>
    /// <param name="path">Path to directory of plugin.</param>
    /// <returns>All types in the assembly that implement the plugin interface.</returns>
    public static List<IPlugin>? LoadPluginFromDirectory(string path)
    {
        // Find manifest file
        string directory = Path.GetFullPath(path);
        if (!Directory.Exists(directory))
            return null;

        // Load manifest file
        using var jsonStream = File.Open(
            Path.Combine(directory, "manifest.json"),
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite
        );

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var manifest = JsonSerializer.Deserialize<Manifest>(jsonStream, jsonOptions);
        if (manifest is null || string.IsNullOrEmpty(manifest.AssemblyPath))
            return null;

        // Load assembly at path from manifest
        var assemblyPath = Path.Combine(directory, manifest.AssemblyPath);
        var assembly = Plugins.LoadPluginAssemblyFromPath(assemblyPath);
        if (assembly is null)
            return null;

        // Create plugin types from assembly
        return Plugins.InstantiateTypesFromPluginAssembly(assembly);
    }


    // /// <summary>
    // /// Searches all subdirectories of pluginDirectory for manifest files to load plugins.
    // /// </summary>
    // /// <param name="pluginDirectoryPath"></param>
    // /// <returns>All types that implement IPlugin.</returns>
    // public static List<IPlugin> LoadAllPluginsFromDirectory(string pluginDirectoryPath)
    // {
    //     var pluginDirectoryInfo = new DirectoryInfo(pluginDirectoryPath);
    //     var plugins = new List<IPlugin>();
    //     foreach (var dir in pluginDirectoryInfo.EnumerateDirectories())
    //     {
    //         var directoryPlugins = LoadPluginsFromDirectory(dir.FullName);
    //         if (directoryPlugins is not null)
    //             plugins.AddRange(directoryPlugins);
    //     }

    //     return plugins;
    // }

    // /// <summary>
    // /// Loads plugin types from the assembly paths defined in manifest.json in the directory at path.
    // /// </summary>
    // /// <param name="path">Path to the directory of the plugin.</param>
    // /// <returns>All types in assembly that implement IPlugin.</returns>
    // public static List<IPlugin>? LoadPluginsFromDirectory(string path)
    // {

    //     var pluginDirectory = new DirectoryInfo(path);
    //     using var jsonStream =
    //         new FileStream(Path.Combine(pluginDirectory.FullName, "manifest.json"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    //     if (jsonStream is null)
    //         return null;

    //     var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    //     var manifest = JsonSerializer.Deserialize<PluginManifest>(jsonStream, jsonOptions);
    //     if (manifest is null)
    //         return null;

    //     return manifest.AssemblyPaths.Select(p => Path.Combine(pluginDirectory.FullName, p)).SelectMany(p => Plugins.LoadPluginAssemblyFromPath(p)).ToList();
    // }
}