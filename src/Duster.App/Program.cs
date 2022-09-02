using Microsoft.Extensions.Configuration;

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
        var systems = coreMods.Select(m => host.InstantiateModSystemProviders(app.World, m));

        // Add systems from the system providers
        foreach (var si in systems)
        {
            app.AddModSystem(si.System);
        }

        // Start systems from core mods
        app.Run();

        // Load instance mods
        var mods = loader.LoadInstanceMods(host.LoadContext, )

        // Ensure that at least one of each required mod is available.

        // Search mod folders for a list of available mods, their information, and the path to their directory.
        // Get all instances
        // Allow user to choose instance
        // This means we must have a core mod concept for mods that must load for any instance.
        // I can still make the core mods changable, but some will have to 
        // be there, windowing and rendering for example.
    }
}
