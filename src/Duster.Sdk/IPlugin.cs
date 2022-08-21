using DefaultEcs;
using DefaultEcs.System;

namespace Duster.Sdk;

public interface IPlugin : IDisposable
{
    string Name { get; }
    string Description { get; }
    List<ISystem<float>> FrameUpdateSystems { get; }
    List<ISystem<float>> FixedUpdateSystems { get; }
    void Initialize(World world);
}

public interface IPluginContextFactory
{
    IPluginContext GetContext();
}

public interface IPluginContext
{
    List<ISystem<float>> FrameUpdateSystems { get; }
    List<ISystem<float>> FixedUpdateSystems { get; }
}

public enum SystemType
{
    FrameUpdate,
    FixedUpdate
}

public interface ISystemInfo
{
    ISystem<float> System { get; }
    SystemType SystemType { get; }
}