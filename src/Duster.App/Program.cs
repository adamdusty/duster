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

        using var app = new Application();
        var modLoader = new ModLoader();

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
