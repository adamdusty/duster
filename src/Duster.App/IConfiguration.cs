using Duster.ModLoading;

namespace Duster.App;

public interface IConfig
{
    string ModDirectory { get; }
    List<ModListEntry> CoreMods { get; }
}

public class Config : IConfig
{
    public string ModDirectory { get; private set; }
    public List<ModListEntry> CoreMods { get; private set; }

    public Config(string modDirectory, List<ModListEntry> coreMods)
    {
        ModDirectory = modDirectory;
        CoreMods = coreMods;
    }
}
