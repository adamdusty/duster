using System.Reflection;
using System.Diagnostics;
using System.IO.Abstractions;
using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;
using Duster.ModLoading;

namespace Duster.App;

class Program
{
    static async Task Main()
    {
        // Load all available plugins
        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
        var modDir = Path.Combine(exeDir, "mods");

        var app = new Application();
        var host = new ModHost();
        var mods = await host.LoadModsFromDirectory(modDir);

        System.Console.WriteLine(mods.Count);

        // loader.GetAssemblyPaths()

        // var mods = await loader.LoadMods(modDir);
        // System.Console.WriteLine($"Loaded mod count: {mods?.Count}");

        // // Create instances of factories
        // List<ISystemProvider> providers = mods?.Select(m => m.Assembly)
        //     .SelectMany(a => a.GetTypes())
        //     .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
        //     .Select(t => Activator.CreateInstance(t) as ISystemProvider)
        //     .Where(sp => sp is not null)
        //     .ToList()!;

        // // Create instances of system info
        // var systems = providers.Select(p => p.GetSystemInfo(app.World));

        // Configure application systems
        // foreach (var info in systems)
        // {
        //     if (info.UpdateFrequency == UpdateFrequency.Fixed)
        //         app.FixedUpdateSystems.Add(info.System);
        //     else if (info.UpdateFrequency == UpdateFrequency.Frame)
        //         app.FrameUpdateSystems.Add(info.System);
        // }

        // Start application
        app.Run();
    }
}
