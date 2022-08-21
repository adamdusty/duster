namespace Duster.Sdk;

public class Manifest
{
    public string ModName { get; }
    public string AssemblyPath { get; }

    public Manifest(string modName, string assemblyPath)
    {
        ModName = modName;
        AssemblyPath = assemblyPath;
    }
}