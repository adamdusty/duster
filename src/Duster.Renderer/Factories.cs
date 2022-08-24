using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;

public class WindowSystemFactory : ISystemProvider
{
    public SystemInfo GetSystemInfo() => new SystemInfo { Parallel = true };
    public ISystem<float> GetSystem(World world) => new WindowSystem(world);
}