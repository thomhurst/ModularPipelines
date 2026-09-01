using ModularPipelines.Models;
using ModularPipelines.Yarn.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Yarn.UnitTests;

public class YarnOptionsTests
{
    [Test]
    public async Task Prerelease_Renders_Bare_And_Explicit_Forms()
    {
        await AssertArguments(
            BuildArguments(new YarnVersionApplyOptions { Prerelease = CliOptionValue.Bare }),
            ["--prerelease"]);
        await AssertArguments(
            BuildArguments(new YarnVersionApplyOptions { Prerelease = "rc.1" }),
            ["--prerelease=rc.1"]);
    }

    [Test]
    public async Task Dlx_Renders_Yarn_Options_Before_Command()
    {
        var arguments = BuildArguments(new YarnDlxOptions(["ts-node", "--transpile-only"])
        {
            Package = ["typescript"],
            Quiet = true,
        });

        await AssertArguments(arguments,
        [
            "--package", "typescript",
            "--quiet",
            "ts-node", "--transpile-only",
        ]);
    }
}
