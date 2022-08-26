using System.Reflection;
using Duster.Sdk;

namespace Duster.ModLoader;


public class ModService
{
    private ModLoader _loader;
    public ModService(IEnumerable<string> searchPaths)
    {
        _loader = new ModLoader(searchPaths);
    }

    /// <summary>
    /// Load all assemblies of mods in a directory.
    /// </summary>
    /// <param name="directory">Path to directory with mods.</param>
    /// <returns></returns>
    public async Task<List<ModInfo>?> LoadMods(string directory)
    {
        if (!Directory.Exists(directory))
        {
            // Log that mod directory doesn't exist and return null
            System.Console.WriteLine("Path doesn't exist");
            return null;
        }

        var modLoadTasks = new List<Task<ModInfo?>>();
        foreach (var mod in Directory.EnumerateDirectories(directory))
        {
            modLoadTasks.Add(_loader.LoadMod(mod));
        }

        var mods = await Task.WhenAll(modLoadTasks);
        if (mods is null)
        {
            System.Console.WriteLine("No mods loaded");
            return null;
        }

        return mods.Where(m => m is not null).ToList()!;
    }
}