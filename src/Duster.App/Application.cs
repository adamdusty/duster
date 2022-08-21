using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.App;

public class Application : IDisposable
{
    private List<ISystem<float>> _fixedUpdateSystems;
    private List<ISystem<float>> _frameUpdateSystems;
    private List<IPlugin> _plugins;

    public World World { get; private set; }

    public Application()
    {
        World = new World();
        World.Set<ApplicationState>(new ApplicationState());
        _fixedUpdateSystems = new List<ISystem<float>>();
        _frameUpdateSystems = new List<ISystem<float>>();
        _plugins = new List<IPlugin>();
    }

    public void LoadAllPlugins(string path)
    {
        _plugins = PluginLoading.LoadPluginsFromDirectory(path) ?? new List<IPlugin>();
        foreach (var p in _plugins)
        {
            p.Initialize(World);
            _fixedUpdateSystems.AddRange(p.FixedUpdateSystems);
            _frameUpdateSystems.AddRange(p.FrameUpdateSystems);
        }
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
        foreach (var p in _plugins)
        {
            p.Dispose();
        }
    }
}