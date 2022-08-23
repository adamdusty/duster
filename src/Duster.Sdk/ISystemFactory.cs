using DefaultEcs;
using DefaultEcs.System;

namespace Duster.Sdk;

public interface ISystemFactory
{
    public ISystem<float> CreateSystem(World world);
}