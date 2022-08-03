namespace Duster.Sdk;

public interface IPlugin
{
    public string Name { get; }
    public Version Version { get; }

    public void Load();
    public void Unload();
}
