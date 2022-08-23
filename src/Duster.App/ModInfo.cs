using System.Reflection;

namespace Duster.App;

public class ModInfo
{
    public Assembly Assembly { get; init; }
    public DirectoryInfo Directory { get; init; }
    public string Name { get; init; }
    public bool Enabled { get; set; }


    public ModInfo(Assembly assembly, string path, string name)
    {
        Assembly = assembly;
        Directory = new DirectoryInfo(path);
        Name = name;
        Enabled = false;
    }
}