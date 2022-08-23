using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;

public class WindowSystemFactory : ISystemFactory
{
    public ISystem<float> CreateSystem(World world)
    {
        return new WindowSystem(world);
    }
}