using ModularPipelines.TestHelpers;
using ModularPipelines.WinGet.Options;

namespace ModularPipelines.WinGet.UnitTests;

public class WinGetGlobalOptionsTests : TestBase
{
    [Test]
    public async Task List_Renders_Aliased_Flags_And_Repeated_Sort_Values()
    {
        var rendered = await RenderCommand(new WingetListOptions
        {
            Asc = true,
            Pinned = true,
            Sort = ["name", "version"],
            Unknown = true,
        });

        await Assert.That(rendered).IsEqualTo("winget list --unknown --pinned --sort name --sort version --asc");
    }

    [Test]
    public async Task Search_Renders_Inherited_Options_After_Command()
    {
        var rendered = await RenderCommand(new WingetSearchOptions
        {
            Query = "example",
            Proxy = "https://proxy.example",
            Verbose = true,
            Nowarn = true,
        });

        await Assert.That(rendered).IsEqualTo(
            "winget search --query example --nowarn --proxy https://proxy.example --verbose");
        await Assert.That(typeof(WingetSearchOptions).GetProperty(nameof(WingetOptions.Verbose))!.DeclaringType)
            .IsEqualTo(typeof(WingetOptions));
    }
}
