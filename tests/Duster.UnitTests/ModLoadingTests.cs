using Xunit;
using Duster.ModLoading;
using System.IO.Abstractions.TestingHelpers;

namespace Duster.UnitTests;

public class ModLoadingTests
{
    [Fact]
    public async Task ModFinder_ReturnsExpectedModInfos()
    {
        var fs = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"./mods/test-mod1/manifest.json", new MockFileData(@"{ ""modName"": ""TestMod1"", ""assemblyPath"": ""./TestMod1.dll"" }")},
            {@"./mods/test-mod2/manifest.json", new MockFileData(@"{ ""modName"": ""TestMod2"", ""assemblyPath"": ""./TestMod2.dll"" }")}
        });

        var modFinder = new ModLoader(fs);

        var infos = await modFinder.GetInformationForAllMods(@"./mods");
        Assert.NotNull(infos);
        Assert.NotEmpty(infos);
        infos = infos!.OrderBy(i => i.ModName).ToList();

        Assert.Equal(infos[0].ModName, "TestMod1");
        Assert.Equal(
            fs.Path.GetFullPath(infos[0].AssemblyPath),
            fs.Path.GetFullPath(@"./mods/test-mod1/TestMod1.dll")
        );

        Assert.Equal(infos[1].ModName, "TestMod2");
        Assert.Equal(
            fs.Path.GetFullPath(infos[1].AssemblyPath),
            fs.Path.GetFullPath(@"./mods/test-mod2/TestMod2.dll")
        );
    }

    [Fact]
    public async void ModFinder_ReturnsNullIfNoManifests()
    {
        var fs = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"./mods/test-mod1/junk_file.json", new MockFileData(@"{ ""modName"": ""TestMod1"", ""assemblyPath"": ""./TestMod1.dll"" }")},
        });

        var modFinder = new ModLoader(fs);
        var infos = await modFinder.GetInformationForAllMods(@"./mods");

        Assert.Null(infos);
    }
}