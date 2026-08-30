using ModularPipelines.Context;
using ModularPipelines.Node.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Node.UnitTests.Api;

public class PnpmCommandRenderingTests : TestBase
{
    [Test]
    public async Task Stage_Publish_Renders_Dry_Run_And_Json_As_Bare_Flags()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PnpmStagePublishOptions
        {
            Tarball = "package.tgz",
            DryRun = true,
            Json = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pnpm stage publish --dry-run --json package.tgz");
    }
}
