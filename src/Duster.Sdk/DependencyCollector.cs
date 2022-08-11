namespace Duster.Sdk;

public class DependencyCollector
{
    private DirectoryInfo _pluginDirectory;

    public DependencyCollector(string dependencyDirectory)
    {
        _pluginDirectory = new DirectoryInfo(dependencyDirectory);
    }

    public List<string> CollectDependencies(PluginManifest manifest)
    {
        return manifest.Dependencies.Select(d => FindDependencyPath(d)).Where(p => p is not null).ToList()!;

    }

    public string? FindDependencyPath(Dependency dep)
    {
        foreach (var dir in _pluginDirectory.EnumerateDirectories())
        {
            if (dir.Name == dep.DirectoryName())
                return dir.FullName;
        }

        return null;
    }
}