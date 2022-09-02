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
    public string Id;
    public Assembly Assembly;

    public Mod(string @namespace, string name, Assembly assembly)
    {
        Id = $"{@namespace}:{name}";
        Assembly = assembly;
    }
}