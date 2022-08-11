using System.Collections.Generic;

namespace Duster.Sdk;

public class PluginManifest
{
    private List<Dependency> _dependencies;

    public string Name { get; }
    public string Description { get; }
    public Sdk.Version Version { get; }
    public IReadOnlyList<Dependency> Dependencies => _dependencies;

    public PluginManifest(string name, string desc, Sdk.Version version, IEnumerable<Dependency> deps)
    {
        _dependencies = new List<Dependency>(deps);
        Name = name;
        Description = desc;
        Version = version;
    }
}