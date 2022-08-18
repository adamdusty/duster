using DefaultEcs;
using DefaultEcs.System;

namespace Duster.Sdk;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    List<ISystem<float>> FrameUpdateSystems { get; }
    List<ISystem<float>> FixedUpdateSystems { get; }
    void Initialize(World world);
}