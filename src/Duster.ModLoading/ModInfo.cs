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

public record Mod
{
    public Manifest Manifest { get; init; }
    public Assembly Assembly { get; init; }
    public List<SystemInfo> Systems { get; private set; }

    public Mod(Manifest manifest, Assembly assembly)
    {
        Manifest = manifest;
        Assembly = assembly;
        Systems = new List<SystemInfo>();
    }
}