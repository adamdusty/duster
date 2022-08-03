using Duster.Sdk;
using Duster.TestPlugin;

namespace Duster.TestPlugin2;

public class TestPlugin2 : IPlugin
{
    public string Name => "Test Plugin 2";

    public Sdk.Version Version => new(0, 1, 0);

    public void Load()
    {
        Console.WriteLine(Library.Greet());
    }

    public void Unload() { }
}
