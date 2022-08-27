using System.Text.Json;
using System.IO.Abstractions;
using Duster.Sdk;

namespace Duster.ModLoading;

public class ModFinder
{
    private IFileSystem _fileSystem;

    public ModFinder(IFileSystem fileSystem) { _fileSystem = fileSystem};
    public ModFinder() : this(new FileSystem()) { }

    public async Task<List<ModInfo>?> GetModAssemblyPaths(string dir)
    {
        if (!_fileSystem.Directory.Exists(dir))
            return null;

        var files = _fileSystem.Directory.EnumerateDirectories(dir)
            .Select(d => _fileSystem.Path.Combine(d, "manifest.json"))
            .Where(p => _fileSystem.File.Exists(p));

        var manifests = await Task.WhenAll(files.Select(async f => await ReadManifest(f)));
        if (manifests is null)
            return null;


        // This is ugly but I can't currently come up with another way 
        // to keep the directory path with the manifest object without using a foreach
        // var manifests = await Task.WhenAll(files.Select(async f => (_fileSystem.Path.GetDirectoryName(f), await ReadManifest(f))));
        // var modInfos = manifests.Where(m => m.Item1 is not null && m.Item2 is not null)
        //     .Select(m => _fileSystem.Path.Combine(m.Item1!, m.Item2!.AssemblyPath))
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
            using var json = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await JsonSerializer.DeserializeAsync<Manifest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            return null;
        }
    }
}