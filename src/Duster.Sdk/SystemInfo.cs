using DefaultEcs.System;

namespace Duster.Sdk;

public struct SystemInfo
{
    public bool Parallel;
    public UpdateFrequency UpdateFrequency;
    public ISystem<float> System;
}