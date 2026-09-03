using ModularPipelines.Chocolatey.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Chocolatey.UnitTests;

public class ChocolateyOptionsTests : TestBase
{
    [Test]
    public async Task Default_Action_Commands_Render_Without_Operands()
    {
        object[] options =
        [
            new ChocoListOptions(),
            new ChocoFeatureOptions(),
            new ChocoPinOptions(),
            new ChocoSourceOptions(),
        ];

        foreach (var option in options)
        {
            await AssertArguments(BuildArguments(option), []);
        }
    }

    [Test]
    public async Task Optional_Structured_Operands_Render_In_Position()
    {
        await AssertArguments(
            BuildArguments(new ChocoListOptions { Filter = "git" }),
            ["git"]);
        await AssertArguments(
            BuildArguments(new ChocoFeatureOptions { List = "enable" }),
            ["enable"]);
    }

    [Test]
    public async Task Package_Group_Renders_Each_Package_As_A_Separate_Argument()
    {
        var arguments = BuildArguments(new ChocoInstallOptions("first")
        {
            Pkg2PkgN = ["second", "third"],
        });

        await AssertArguments(arguments, ["first", "second", "third"]);
    }

    [Test]
    public async Task Template_Properties_Render_As_Separate_Arguments()
    {
        var arguments = BuildArguments(new ChocoNewOptions("sample")
        {
            PropertyValuePropertyNValueN = ["Owner=team", "Port=443"],
        });

        await AssertArguments(arguments, ["sample", "Owner=team", "Port=443"]);
    }

    [Test]
    public async Task Pack_Nuspec_Path_Precedes_Property_Value()
    {
        var arguments = BuildArguments(new ChocoPackOptions
        {
            PathToNuspec = "sample.nuspec",
            PropertyValue = "Version=1",
        });

        await AssertArguments(arguments, ["sample.nuspec", "Version=1"]);
    }
}
