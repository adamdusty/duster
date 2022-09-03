using System.Reflection;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.ModLoading;

public record ModInfo
{
    public Manifest Manifest { get; private set; }
    public string Directory { get; private set; }

    public ModInfo(Manifest manifest, string dir)
    {
        Manifest = manifest;
        Directory = dir;
    }
}

public class Mod
{
    public readonly string Namespace;
    public readonly string Name;
    public Assembly Assembly;

    public Mod(string @namespace, string name, Assembly assembly)
    {
        Namespace = @namespace;
        Name = name;
        Assembly = assembly;
    }
}