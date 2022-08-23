using System.Reflection;
using System.Diagnostics;
using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static async Task Main()
    {
        // Load all available plugins
        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
        var modDir = Path.Combine(exeDir, "mods");

        var app = new Application(exeDir);
        var loader = new ModLoader(Directory.GetDirectories(modDir));
        var service = new ModService();
        System.Console.WriteLine(modDir);
        var mods = await service.LoadMods(loader, modDir);
        if (mods is not null)
        {
            foreach (var m in mods)
            {
                System.Console.WriteLine($"Mod Name: {m.Name}");
            }
        }

        // Load mod assemblies
        // 
        // Create applciation sequential systems and parallel systems from enabled mods

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
