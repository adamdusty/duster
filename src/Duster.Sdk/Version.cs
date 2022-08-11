namespace Duster.Sdk;

public class Version
{
    public uint Major { get; }
    public uint Minor { get; }
    public uint Patch { get; }

    public Version(uint major, uint minor, uint patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}