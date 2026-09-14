using ModularPipelines.Context;
using ModularPipelines.Node.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Node.UnitTests.Api;

public class PnpmCommandRenderingTests : TestBase
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Cpu_Values_Repeat_The_Switch(bool create)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PnpmOptions options = create
            ? new PnpmCreateOptions { Cpu = ["x64", "arm64"] }
            : new PnpmDlxOptions { Cpu = ["x64", "arm64"] };

        var commandLine = builder.Build(options);

        await Assert.That(commandLine.ToString())
            .IsEqualTo($"pnpm {(create ? "create" : "dlx")} --cpu x64 --cpu arm64");
    }

    [Test]
    public async Task Stage_Publish_Renders_Dry_Run_And_Json_As_Bare_Flags()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PnpmStageOptions
        {
            Params = ["publish", "package.tgz"],
            DryRun = true,
            Json = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pnpm stage --dry-run --json publish package.tgz");
    }

    [Test]
    public async Task Audit_Renders_Signatures_As_A_Parent_Parameter()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PnpmAuditOptions
        {
            Params = ["signatures"],
            Json = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pnpm audit --json signatures");
    }
}
