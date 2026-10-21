using ModularPipelines.DotNet.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetBuildOptionsTests
{
    [Test]
    [Arguments("build")]
    [Arguments("clean")]
    [Arguments("pack")]
    [Arguments("publish")]
    public async Task NoLogo_Properties_Render_Normalized_Switch(string command)
    {
        object options = command switch
        {
            "build" => new DotNetBuildOptions { NoLogo = true },
            "clean" => new DotNetCleanOptions { NoLogo = true },
            "pack" => new DotNetPackOptions { NoLogo = true },
            "publish" => new DotNetPublishOptions { NoLogo = true },
            _ => throw new ArgumentOutOfRangeException(nameof(command)),
        };

        var arguments = BuildArguments(options);

        // Both SDK help shapes are valid: a presence flag or an explicit boolean value.
        // The generator tests enforce true/false/null rendering for true-default help.
        await Assert.That(arguments.SequenceEqual(["--nologo"])
                          || arguments.SequenceEqual(["--nologo", "true"])).IsTrue();
    }
}
