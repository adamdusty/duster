using System.Diagnostics;
using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.App;

class Program
{
    static void Main()
    {
        // Load all available plugins
        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;

        using var app = new Application();
        app.LoadAllPlugins(Path.Combine(exeDir, "plugins"));

        // Set up timing
        var sw = new Stopwatch();
        sw.Start();

        var time = 0d;
        var deltaTime = 0.02d;
        var frameBeginTimeStamp = sw.Elapsed.TotalSeconds;
        var frameEndTimeStamp = frameBeginTimeStamp;
        var frameTime = frameEndTimeStamp - frameBeginTimeStamp;
        var accumulator = 0d;
        var interpolatedDeltaTime = 1.0d;

        double t0 = 0;

        // Game loop
        while (!app.World.Get<bool>())
        {
            frameBeginTimeStamp = frameEndTimeStamp; // Beginning of new frame
            accumulator += frameTime;
            time = 0;

            // Fixed dt update
            while (accumulator >= deltaTime)
            {
                t0 = sw.Elapsed.TotalSeconds;
                app.FixedUpdate((float)deltaTime);
                accumulator -= deltaTime;
                time += deltaTime;
            }

            interpolatedDeltaTime = accumulator / deltaTime;

            // Variable dt update
            app.FrameUpdate((float)interpolatedDeltaTime);

            frameEndTimeStamp = sw.Elapsed.TotalSeconds; // End of previous frame 
            frameTime = frameEndTimeStamp - frameBeginTimeStamp; // Time previous frame took to update
        }
    }
}
