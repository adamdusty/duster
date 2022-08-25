using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;

public class WindowSystemFactory : ISystemProvider
{
    public SystemInfo GetSystemInfo(World world) => new SystemInfo
    {
        Parallel = true,
        UpdateFrequency = UpdateFrequency.Frame,
        System = new WindowSystem(world)
    };
}