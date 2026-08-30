using ModularPipelines.Yarn.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Yarn.UnitTests;

public class YarnOptionsTests
{
    [Test]
    public async Task Dlx_Renders_Yarn_Options_Before_Command()
    {
        var arguments = BuildArguments(new YarnDlxOptions(["ts-node", "--transpile-only"])
        {
            Package = ["typescript"],
            Quiet = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--package", "typescript",
            "--quiet",
            "ts-node", "--transpile-only",
        ]);
    }
}
