using DefaultEcs.System;

namespace Duster.Sdk;

public readonly struct SystemInfo
{
    public readonly bool Parallel { get; init; }
    public readonly UpdateFrequency UpdateFrequency { get; init; }
    public readonly ISystem<float> System { get; init; }
}

public enum UpdateFrequency
{
    Frame,
    Fixed,
}