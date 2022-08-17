using System.Diagnostics;
using DefaultEcs;
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

        // Initialize plugins
        foreach (var p in plugins)
        {
            p.Initialize(world);
        }

        // Set up timing
        var sw = new Stopwatch();
        sw.Start();

        var dt = 0d;
        var cycleBeginTime = sw.Elapsed.TotalSeconds;
        var cycleEndTime = cycleBeginTime;
        var frameTime = cycleEndTime;


        // Game loop
        while (true)
        {
            cycleBeginTime = sw.Elapsed.TotalSeconds;

            while (frameTime > 0)
            {
                dt = Math.Min(frameTime, dt);
                frameTime -= dt;
            }

            foreach (var p in plugins)
            {
                foreach (var s in p.Systems)
                {
                    s.Update((float)dt);
                }
            }

            cycleEndTime = sw.Elapsed.TotalSeconds;
            dt = cycleEndTime - cycleBeginTime;
        }
    }
}
