using System.Reflection;

namespace Duster.ModLoader;

public class ModInfo
{
    public Assembly Assembly { get; init; }
    public string Directory { get; init; }
    public string Name { get; init; }
    public bool Enabled { get; set; }


    public ModInfo(Assembly assembly, string path, string name)
    {
        Assembly = assembly;
        Directory = path;
        Name = name;
        Enabled = false;
    }
}