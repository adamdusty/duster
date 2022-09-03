using Duster.Sdk;
using Duster.ModLoading;

namespace Duster.App;

class Program
{
    static void Main()
    {
        // Load all available plugins
        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
        var modDir = Path.Combine(exeDir, "mods");

        var configLoader = new ConfigLoader();
        var config = configLoader.LoadConfig("./config.json");

        var app = new Application(config);
        var host = new ModHost();

        // Load core mods
        // Get core mod list from config
        // Loop through each mod in the mod directory and if the mod matches an entry on the core mod list, attempt to load it.
        var coreMods = host.LoadMods(app.Config.CoreMods);

        // Get system providers from core mods
        var providers = coreMods.SelectMany(m => m.Assembly.GetTypes())
            .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as ISystemProvider)
            .Where(sp => sp is not null);

        // Add systems from the system providers
        foreach (var p in providers)
        {
            if (p is not null)
                app.AddModSystem(p.GetSystemInfo());
        }

        // Start systems from core mods
        app.Run();

        // Load sequence
        // Look for config, if unavailable create default
        // Create application with configuration
        // Get core mods that must be loaded before everything else
        //      - What to do if a core mod is unavailable?
        // Load core mod assemblies
    }
}
