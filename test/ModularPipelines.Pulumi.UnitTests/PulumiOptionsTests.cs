using ModularPipelines.Context;
using ModularPipelines.Pulumi.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Pulumi.UnitTests;

public class PulumiOptionsTests : TestBase
{
    [Test]
    public async Task Hoists_EqualsSeparated_Option_Before_Provider_Parameters()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiPackageInfoOptions("aws")
        {
            ProviderParameter = ["param"],
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pulumi package info --color=never aws -- param");
    }

    [Test]
    public async Task Legacy_Env_Run_Constructor_Uses_Manual_Command_Operand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiEnvRunOptions("development")
        {
            Arguments = ["echo", "hello"],
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pulumi env run development -- echo hello");
    }

    [Test]
    public async Task Legacy_Logout_Local_Flag_Uses_Local_Backend_Url()
    {
        var builder = await GetService<ICommandLineBuilder>();

#pragma warning disable CS0618
        var enabled = builder.Build(new PulumiLogoutOptions { Local = true });
        var disabled = builder.Build(new PulumiLogoutOptions { Local = false });
#pragma warning restore CS0618

        using (Assert.Multiple())
        {
            await Assert.That(enabled.ToString()).IsEqualTo("pulumi logout --local=file://~");
            await Assert.That(disabled.ToString()).IsEqualTo("pulumi logout");
        }
    }
}
