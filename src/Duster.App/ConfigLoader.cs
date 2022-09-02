using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Duster.App;

public class ConfigLoader
{
    private IFileSystem _fileSystem;

    public ConfigLoader(IFileSystem fs) { _fileSystem = fs; }
    public ConfigLoader() : this(new FileSystem()) { }

    public IConfig LoadConfig(string path)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(path, optional: false)
            .Build();

        return config.Get<Config>();
    }
}