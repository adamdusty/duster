using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;

namespace Duster.TestPlugin;

public class TestSystem : AEntitySetSystem<float>
{
    public TestSystem(World world) : base(world.GetEntities().AsSet()) { }

    protected override void Update(float state, ReadOnlySpan<Entity> entities)
    {
        System.Console.WriteLine("hello");
    }
}

public class TestPlugin : IPlugin
{
    public string Name => "TestPlugin";

    public string Description => "Plugin to test integration.";

    public List<ISystem<float>> Systems { get; private set; }

    public TestPlugin()
    {
        Systems = new List<ISystem<float>>();
    }

    public void Initialize(World world)
    {
        Systems.Add(new TestSystem(world));
        System.Console.WriteLine("Initializing plugin");
    }
}