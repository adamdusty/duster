namespace Duster.Sdk;

public interface IPlugin
{
    public string Name { get; }
    public Version Version { get; }

    public void Init();
    public void Execute();
    public void Deinit();
}
