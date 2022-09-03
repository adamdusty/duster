using DefaultEcs;

namespace Duster.Sdk;

public interface ISystemProvider
{
    SystemInfo GetSystemInfo();
}