namespace Duster.ModLoading;

public record struct ModListEntry
{
    public string Namespace;
    public string Name;
    public Version Version;
}