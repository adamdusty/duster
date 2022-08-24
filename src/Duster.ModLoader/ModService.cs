using System.Reflection;
using Duster.Sdk;

namespace Duster.ModLoader;


public class ModService
{
    public ModService() { }

    public async Task<List<ModInfo>?> LoadMods(ModLoader loader, string path)
    {
        if (!Directory.Exists(path))
        {
            // Log that mod directory doesn't exist and return null
            System.Console.WriteLine("Path doesn't exist");
            return null;
        }

        var modLoadTasks = new List<Task<ModInfo?>>();
        foreach (var mod in Directory.EnumerateDirectories(path))
        {
            modLoadTasks.Add(loader.LoadMod(mod));
        }

        var mods = await Task.WhenAll(modLoadTasks);
        if (mods is null)
        {
            System.Console.WriteLine("No mods loaded");
            return null;
        }

        return mods.Where(m => m is not null).ToList()!;
    }

    // public List<ISystemFactory> GetSystemFactoriesFromEnabledMods(IEnumerable<ModInfo> mods)
    // {
    //     return mods.Where(m => m.Enabled == true)
    //         .Select(m => m.Assembly)
    //         .SelectMany(a => a.GetTypes())
    //         .Where(t => typeof(ISystemFactory).IsAssignableFrom(t))
    //         .Select(t => Activator.CreateInstance(t) as ISystemFactory)
    //         .Where(f => f is not null)
    //         .ToList()!;
    // }

    // public List<ISystemFactory> GetSystemFactoriesFromEnabledMods()
    // {
    //     return _mods.Values.Where(m => m.Enabled == true)
    //         .Select(m => m.Assembly)
    //         .SelectMany(a => a.GetTypes())
    //         .Where(t => typeof(ISystemFactory).IsAssignableFrom(t))
    //         .Select(t => Activator.CreateInstance(t) as ISystemFactory)
    //         .Where(f => f is not null)
    //         .ToList()!;
    // }
}