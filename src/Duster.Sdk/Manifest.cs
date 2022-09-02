namespace Duster.Sdk;

public class Manifest
{
    public string Namespace { get; set; }
    public string Id { get; set; }
    public string DisplyName { get; set; }
    public Version Version { get; set; }

    public Manifest(string @namespace, string id, string displayName, Version version)
    {
        Namespace = @namespace;
        Id = id;
        DisplyName = displayName;
        Version = version;
    }
}