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

        var testMod1 = infos!.Where(i => i.Manifest.ModName == "TestMod1").FirstOrDefault();
        var testMod2 = infos!.Where(i => i.Manifest.ModName == "TestMod2").FirstOrDefault();
        Assert.NotNull(testMod1);
        Assert.NotNull(testMod2);

        Assert.Equal("TestMod1", testMod1!.Manifest.ModName);
        Assert.Equal(
            fs.Path.GetFullPath(@"./TestMod1.dll"),
            fs.Path.GetFullPath(testMod1!.Manifest.AssemblyPath)
        );

        Assert.Equal("TestMod2", testMod2!.Manifest.ModName);
        Assert.Equal(
            fs.Path.GetFullPath(@"./TestMod2.dll"),
            fs.Path.GetFullPath(testMod2!.Manifest.AssemblyPath)
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