using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.App;

public class Application : IDisposable
{
    private List<ISystem<float>> _fixedUpdateSystems;
    private List<ISystem<float>> _frameUpdateSystems;

    public World World { get; private set; }

    public Application()
    {
        World = new World();
        World.Set<ApplicationState>(new ApplicationState());
        _fixedUpdateSystems = new List<ISystem<float>>();
        _frameUpdateSystems = new List<ISystem<float>>();
    }

    public void FixedUpdate(float dt)
    {
        foreach (var sys in _fixedUpdateSystems)
        {
            sys.Update(dt);
        }
    }

    public void FrameUpdate(float dt)
    {
        foreach (var sys in _frameUpdateSystems)
        {
            sys.Update(dt);
        }
    }

    public void Dispose()
    {
    }
}