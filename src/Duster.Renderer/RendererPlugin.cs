using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;
public class RendererPlugin : IPlugin
{
    public string Name => "Renderer";

    public string Description => "Core renderer.";

    public List<ISystem<float>> FrameUpdateSystems { get; private set; }

    public List<ISystem<float>> FixedUpdateSystems { get; private set; }

    public RendererPlugin()
    {
        FrameUpdateSystems = new List<ISystem<float>>();
        FixedUpdateSystems = new List<ISystem<float>>();
    }

    public void Initialize(World world)
    {
        System.Console.WriteLine("Loading render plugin");
        FrameUpdateSystems.Add(new WindowSystem(world));
    }
    public void Dispose() { }
}
