using System.Reflection;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;

namespace Duster.App;

public class Application
{
    private string _applicationDirectory;
    private ParallelSystem<float>? _parallelFixedUpdateSystem;
    private ParallelSystem<float>? _parallelFrameUpdateSystem;
    private SequentialSystem<float>? _sequentialFixedUpdateSystem;
    private SequentialSystem<float>? _sequentialFrameUpdateSystem;


    public World World { get; private set; }

    public Application(string path)
    {
        World = new World();
        World.Set<ApplicationState>(new ApplicationState());
        _applicationDirectory = path;
    }

    public void BuildSystems(IEnumerable<SystemInfo> systems)
    {
        List<ISystem<float>> parallelFixed = new();
        List<ISystem<float>> parallelFrame = new();
        List<ISystem<float>> sequentialFixed = new();
        List<ISystem<float>> sequentialFrame = new();

        foreach (var info in systems)
        {
            if (info.Parallel)
            {
                if (info.UpdateFrequency == UpdateFrequency.Fixed)
                    parallelFixed.Add(info.System);
                else if (info.UpdateFrequency == UpdateFrequency.Frame)
                    parallelFrame.Add(info.System);
            }
            else
            {
                if (info.UpdateFrequency == UpdateFrequency.Fixed)
                    sequentialFixed.Add(info.System);
                else if (info.UpdateFrequency == UpdateFrequency.Frame)
                    sequentialFrame.Add(info.System);
            }
        }

        _sequentialFixedUpdateSystem = new SequentialSystem<float>(sequentialFixed);
        _sequentialFrameUpdateSystem = new SequentialSystem<float>(sequentialFrame);
        _parallelFixedUpdateSystem = new ParallelSystem<float>(
            new DefaultParallelRunner(Environment.ProcessorCount),
            parallelFixed
        );
        _parallelFrameUpdateSystem = new ParallelSystem<float>(
            new DefaultParallelRunner(Environment.ProcessorCount),
            parallelFrame
        );
    }

    public void FixedUpdate(float dt)
    {
        _parallelFixedUpdateSystem?.Update(dt);
        _sequentialFixedUpdateSystem?.Update(dt);
    }

    public void FrameUpdate(float dt)
    {
        _parallelFrameUpdateSystem?.Update(dt);
        _sequentialFrameUpdateSystem?.Update(dt);
    }
}