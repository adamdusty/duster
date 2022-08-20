using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;
using Duster.Renderer;

namespace Duster.TestPlugin;

public class TestSystem : AEntitySetSystem<float>
{
    public TestSystem(World world) : base(world.GetEntities().AsSet()) { }

    protected override void Update(float state, ReadOnlySpan<Entity> entities) { }
}

public class TestPlugin : IPlugin
{
    private DrawInfo _di;
    public string Name => "TestPlugin";

    public string Description => "Plugin to test integration.";

    public List<ISystem<float>> FrameUpdateSystems { get; private set; }
    public List<ISystem<float>> FixedUpdateSystems { get; private set; }

    public TestPlugin()
    {
        FrameUpdateSystems = new List<ISystem<float>>();
        FixedUpdateSystems = new List<ISystem<float>>();
    }

    public void Initialize(World world)
    {
        System.Console.WriteLine("Loading test plugin");
        _di = new DrawInfo();
        FixedUpdateSystems.Add(new TestSystem(world));
    }

    public void Dispose()
    {
        System.Console.WriteLine(_di.X);
    }
}