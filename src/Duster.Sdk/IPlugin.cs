using DefaultEcs;
using DefaultEcs.System;

namespace Duster.Sdk;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    List<ISystem<float>> Systems { get; }
    void Initialize(World world);
}