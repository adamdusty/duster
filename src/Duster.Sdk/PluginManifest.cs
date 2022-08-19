namespace Duster.Sdk;

public class PluginManifest
{
    public string Name { get; }
    public List<string> AssemblyPaths { get; }

    public PluginManifest(string name, List<string> assemblyPaths)
    {
        Name = name;
        AssemblyPaths = assemblyPaths;
    }
}