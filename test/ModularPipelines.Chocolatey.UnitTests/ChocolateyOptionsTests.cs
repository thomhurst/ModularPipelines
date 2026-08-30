using ModularPipelines.Chocolatey.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Chocolatey.UnitTests;

public class ChocolateyOptionsTests : TestBase
{
    [Test]
    public async Task Package_Group_Renders_Each_Package_As_A_Separate_Argument()
    {
        var arguments = BuildArguments(new ChocoInstallOptions("first")
        {
            Pkg2PkgN = ["second", "third"],
        });

        await Assert.That(arguments).IsEquivalentTo(["first", "second", "third"]);
    }

    [Test]
    public async Task Template_Properties_Render_As_Separate_Arguments()
    {
        var arguments = BuildArguments(new ChocoNewOptions("sample")
        {
            PropertyValuePropertyNValueN = ["Owner=team", "Port=443"],
        });

        await Assert.That(arguments).IsEquivalentTo(["sample", "Owner=team", "Port=443"]);
    }

    [Test]
    public async Task Pack_Nuspec_Path_Precedes_Property_Value()
    {
        var arguments = BuildArguments(new ChocoPackOptions
        {
            PathToNuspec = "sample.nuspec",
            PropertyValue = "Version=1",
        });

        await Assert.That(arguments).IsEquivalentTo(["sample.nuspec", "Version=1"]);
    }
}
