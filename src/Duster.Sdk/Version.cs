namespace Duster.Sdk;

public struct Version
{
    public Version(uint major, uint minor, uint patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public uint Major;
    public uint Minor;
    public uint Patch;
}