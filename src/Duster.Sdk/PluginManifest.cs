namespace Duster.Sdk;

public class PluginManifest
{
    public string Name { get; }

    public PluginManifest(string name)
    {
        Name = name;
    }
}