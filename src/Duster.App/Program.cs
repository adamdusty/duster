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

        var loader = new PluginLoader(Path.Combine(exeDir, "plugins"));
        var plugins = loader.LoadAllPlugins();

        // Create game world
        var world = new World();
        var e = world.CreateEntity();

        var fixedUpdateSystems = new List<ISystem<float>>();
        var frameUpdateSystems = new List<ISystem<float>>();

        // Initialize plugins
        foreach (var p in plugins)
        {
            p.Initialize(world);
            System.Console.WriteLine($"Plugin initialized: {p.Name}");
            fixedUpdateSystems.AddRange(p.FixedUpdateSystems);
            frameUpdateSystems.AddRange(p.FrameUpdateSystems);
        }

        var fixedUpdate = new SequentialSystem<float>(fixedUpdateSystems);
        var frameUpdate = new SequentialSystem<float>(frameUpdateSystems);

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
        while (true)
        {
            frameBeginTimeStamp = frameEndTimeStamp; // Beginning of new frame
            accumulator += frameTime;
            time = 0;

            // Fixed dt update
            while (accumulator >= deltaTime)
            {
                t0 = sw.Elapsed.TotalSeconds;
                fixedUpdate.Update((float)deltaTime);
                accumulator -= deltaTime;
                time += deltaTime;
            }

            interpolatedDeltaTime = accumulator / deltaTime;

            // Variable dt update
            frameUpdate.Update((float)interpolatedDeltaTime);

            frameEndTimeStamp = sw.Elapsed.TotalSeconds; // End of previous frame 
            frameTime = frameEndTimeStamp - frameBeginTimeStamp; // Time previous frame took to update
        }
    }
}
