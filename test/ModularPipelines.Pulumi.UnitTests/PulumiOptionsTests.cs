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
    public async Task Env_Run_Uses_Structured_Command_And_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiEnvRunOptions("development", "echo")
        {
            Args = ["hello"],
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pulumi env run development -- echo hello");
    }

    [Test]
    public async Task Env_Run_Structured_Command_Emits_Option_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiEnvRunOptions("development", "bash")
        {
            Args = ["-c", "echo hello"],
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pulumi env run development -- bash -c echo hello");
    }

    [Test]
    public async Task Logout_Local_Option_Uses_Local_Backend_Url()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiLogoutOptions { Local = "file://~" });

        await Assert.That(commandLine.ToString()).IsEqualTo("pulumi logout --local=file://~");
    }
}
