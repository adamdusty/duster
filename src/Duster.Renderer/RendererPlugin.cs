using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;
public class RendererPlugin : IPlugin
{
    private ISystem<float> _system;
    private World _world;

    public RendererPlugin()
    {
        _world = new World();
    }

    public void Initialize()
    {
        _system = new WindowSystem(_world);
    }
}
