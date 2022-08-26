using System.Reflection;
using System.Diagnostics;
using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;
using Duster.ModLoader;

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
        var service = new ModService(Directory.GetDirectories(modDir));

        var mods = await service.LoadMods(modDir);
        System.Console.WriteLine($"Loaded mod count: {mods?.Count}");

        // Create instances of factories
        List<ISystemProvider> providers = mods?.Select(m => m.Assembly)
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ISystemProvider).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as ISystemProvider)
            .Where(sp => sp is not null)
            .ToList()!;

        // Create instances of system info
        var systems = providers.Select(p => p.GetSystemInfo(app.World));

        // Configure application systems
        foreach (var info in systems)
        {
            if (info.UpdateFrequency == UpdateFrequency.Fixed)
                app.FixedUpdateSystems.Add(info.System);
            else if (info.UpdateFrequency == UpdateFrequency.Frame)
                app.FrameUpdateSystems.Add(info.System);
        }

        // Start application


        // Set up timing
        var sw = new Stopwatch();
        sw.Start();

        var time = new TimeTracker(0.02);

        // Game loop
        while (!app.World.Get<ApplicationState>().Quit)
        {
            time.Begin = sw.Elapsed.TotalSeconds;
            time.Accumulator += time.FrameTime;

            // Fixed dt update
            while (time.Accumulator >= time.FixedDeltaTime)
            {
                app.FixedUpdate((float)time.FixedDeltaTime);
                time.Accumulator -= time.FixedDeltaTime;
            }

            time.InterpolatedTime = time.Accumulator / time.FixedDeltaTime;

            // Variable dt update
            app.FrameUpdate((float)time.InterpolatedTime);

            time.End = sw.Elapsed.TotalSeconds;
            time.FrameTime = time.End - time.Begin; // Time previous frame took to update
        }
    }
}
