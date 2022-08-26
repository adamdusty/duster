using System.Reflection;
using System.IO.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;

namespace Duster.App;

public class Application
{
    public List<ISystem<float>> FixedUpdateSystems { get; private set; }
    public List<ISystem<float>> FrameUpdateSystems { get; private set; }
    public World World { get; private set; }

    public Application()
    {
        World = new World();
        World.Set<ApplicationState>(new ApplicationState());

        FixedUpdateSystems = new List<ISystem<float>>();
        FrameUpdateSystems = new List<ISystem<float>>();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.Sources.Clear();
            })
            .ConfigureServices((c, s) =>
            {
                // Register services
                s.AddScoped<IFileSystem, FileSystem>();
            });

        return builder;
    }

    public void FixedUpdate(float dt)
    {
        foreach (var sys in FixedUpdateSystems)
        {
            sys.Update(dt);
        }
    }

    public void FrameUpdate(float dt)
    {
        foreach (var sys in FrameUpdateSystems)
        {
            sys.Update(dt);
        }
    }
}