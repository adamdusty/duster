using DefaultEcs;
using DefaultEcs.System;

namespace Duster.Sdk;

public interface ISystemProvider
{
    public SystemInfo GetSystemInfo();
    public ISystem<float> GetSystem(World world);
}