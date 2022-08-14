namespace Duster.Sdk;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    void Initialize();
}