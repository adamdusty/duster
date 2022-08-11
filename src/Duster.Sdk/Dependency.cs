namespace Duster.Sdk;

public class Dependency
{
    public string Identifier { get; }
    public Sdk.Version Version { get; }

    public Dependency(string id, Sdk.Version version) => (Identifier, Version) = (id, version);

    public string DirectoryName() => $"{Identifier.ToLower()}-{Version}";
}