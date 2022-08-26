using System.Reflection;
using System.Diagnostics;
using System.IO.Abstractions;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;

namespace Duster.App;

public class Application
{
    public List<ISystem<float>> FixedUpdateSystems { get; private set; }
    public List<ISystem<float>> FrameUpdateSystems { get; private set; }
    public World World { get; private set; }

    public Application()
    {
        World = new World();
        World.Set<ApplicationState>(new ApplicationState());

        FixedUpdateSystems = new List<ISystem<float>>();
        FrameUpdateSystems = new List<ISystem<float>>();
    }

    public void Run()
    {
        // Set up timing
        var sw = new Stopwatch();
        sw.Start();

        var time = new TimeTracker(0.02);

        // Game loop
        while (!World.Get<ApplicationState>().Quit)
        {
            time.Begin = sw.Elapsed.TotalSeconds;
            time.Accumulator += time.FrameTime;

            // Fixed dt update
            while (time.Accumulator >= time.FixedDeltaTime)
            {
                FixedUpdate((float)time.FixedDeltaTime);
                time.Accumulator -= time.FixedDeltaTime;
            }

            time.InterpolatedTime = time.Accumulator / time.FixedDeltaTime;

            // Variable dt update
            FrameUpdate((float)time.InterpolatedTime);

            time.End = sw.Elapsed.TotalSeconds;
            time.FrameTime = time.End - time.Begin; // Time previous frame took to update
        }
    }

    public void FixedUpdate(float dt)
    {
        foreach (var sys in FixedUpdateSystems)
        {
            sys.Update(dt);
        }
    }

    public void FrameUpdate(float dt)
    {
        foreach (var sys in FrameUpdateSystems)
        {
            sys.Update(dt);
        }
    }
}