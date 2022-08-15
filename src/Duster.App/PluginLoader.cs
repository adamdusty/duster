using System.Text.Json;
using Duster.Sdk;

namespace Duster.App;

public class PluginLoader
{
    private DirectoryInfo _pluginDirectory;

    public PluginLoader(string path)
    {
        _pluginDirectory = new DirectoryInfo(path);
    }

    public IEnumerable<IPlugin> LoadAllPlugins()
    {
        var assemblyPaths = new List<string>();
        // Load plugin manifest from each folder in ./plugins
        foreach (var dir in _pluginDirectory.EnumerateDirectories())
        {
            // read manifest file
            string manifestJson = File.ReadAllText(Path.Combine(dir.FullName, "manifest.json"));
            var manifest = JsonSerializer.Deserialize<IPluginManifest>(manifestJson);

            if (manifest is null)
                continue;

            // load assembly/types by plugin name described in manifest
            assemblyPaths.Add(Path.Combine(dir.FullName, manifest.Name));
        }

        // Use plugin service to load assembly and types by the plugin name
        return assemblyPaths.SelectMany(a => PluginService.LoadPluginsFromAssembly(a));
    }
}