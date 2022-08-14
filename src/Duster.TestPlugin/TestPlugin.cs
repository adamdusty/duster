using Duster.Sdk;

using static SDL2.SDL;

namespace Duster.TestPlugin;
public class TestPlugin : IPlugin
{
    public string Name => "TestPlugin";
    public string Description => "Test plugin";
    public void Initialize() { }
}