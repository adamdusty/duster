using System.Reflection;
using DefaultEcs;
using DefaultEcs.System;
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

    public void BuildSystems(IEnumerable<ISystem<float>> systems)
    {
        throw new NotImplementedException();
    }

    public void FixedUpdate(float dt)
    {
    }

    public void FrameUpdate(float dt)
    {
    }
}