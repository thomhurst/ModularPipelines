using ModularPipelines.DotNet.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetBuildOptionsTests
{
    [Test]
    public async Task NoLogo_Renders_Switch()
    {
        var options = new DotNetBuildOptions { NoLogo = true };

        var arguments = BuildArguments(options);

        await AssertArguments(arguments, ["--nologo"]);
    }

    [Test]
    [Arguments("clean")]
    [Arguments("pack")]
    [Arguments("publish")]
    public async Task Sibling_NoLogo_Properties_Render_Switch(string command)
    {
        object options = command switch
        {
            "clean" => new DotNetCleanOptions { NoLogo = true },
            "pack" => new DotNetPackOptions { NoLogo = true },
            "publish" => new DotNetPublishOptions { NoLogo = true },
            _ => throw new ArgumentOutOfRangeException(nameof(command)),
        };

        var arguments = BuildArguments(options);

        await AssertArguments(arguments, ["--nologo"]);
    }
}
