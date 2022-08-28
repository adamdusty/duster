using System.Text.Json;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModFinder
{
    private IFileSystem _fileSystem;

    public ModFinder(IFileSystem fileSystem) { _fileSystem = fileSystem; }
    public ModFinder() : this(new FileSystem()) { }


    /// <summary>
    /// If directory <paramref name="dir" /> exists, find all directories within that directory
    /// that contain a manifest.json file. Read the manifest files and return a list of <see cref="ModInfo" />
    /// for each mod folder. If there are no directories with manifest files, return null.
    /// </summary>
    /// <param name="dir">Directory of mods.</param>
    /// <returns>List of <see cref="ModInfo" /> or null</returns>
    public async Task<List<ModInfo>?> GetInformationForAllMods(string dir)
    {
        if (!_fileSystem.Directory.Exists(dir))
            return null;

        var files = _fileSystem.Directory.EnumerateDirectories(dir)
            .Select(d => _fileSystem.Path.Combine(d, "manifest.json"))
            .Where(p => _fileSystem.File.Exists(p));

        var manifests = new ConcurrentBag<(string, Manifest?)>();

        // This is hella ugly but I can't currently come up with another way 
        // to keep the directory path with the manifest object without using a foreach
        await Task.WhenAll(files.Select(async f => manifests.Add((_fileSystem.Path.GetDirectoryName(f), await ReadManifest(f)))));

        if (manifests.IsEmpty)
            return null;

        var modInfos = new List<ModInfo>();
        foreach (var m in manifests.Where(m => m.Item1 is not null && m.Item2 is not null))
        {
            modInfos.Add(
                new ModInfo
                {
                    ModName = m.Item2!.ModName,
                    AssemblyPath = _fileSystem.Path.Combine(m.Item1, m.Item2.AssemblyPath)
                }
            );
        }

        return modInfos;
    }

    /// <summary>
    /// Load manifest file.
    /// </summary>
    /// <param name="path">Path to manifest file.</param>
    /// <returns>Manifest object if successfully read, otherwise null.</returns>
    private async Task<Manifest?> ReadManifest(string path)
    {
        try
        {
            using var json = _fileSystem.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await JsonSerializer.DeserializeAsync<Manifest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            return null;
        }
    }
}