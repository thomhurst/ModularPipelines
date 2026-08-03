using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Node.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Node.UnitTests.Api;

public class NpmCommandRenderingTests : TestBase
{
    [Test]
    public async Task Npm_Options_Use_Subcommand_Attributes()
    {
        var optionsWithFullCommandAttributes = typeof(NpmOptions).Assembly.GetTypes()
            .Where(type => type != typeof(NpmOptions) && type.IsAssignableTo(typeof(NpmOptions)))
            .Where(type => type.GetCustomAttribute<CliCommandAttribute>() is not null)
            .Select(type => type.Name)
            .ToArray();

        await Assert.That(optionsWithFullCommandAttributes).IsEmpty();
    }

    [Test]
    public async Task Token_Revoke_Renders_Npm_Token_Revoke()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmTokenRevokeOptions("example-token"));

        await Assert.That(commandLine.ToString()).IsEqualTo("npm token revoke example-token");
    }

    [Test]
    public async Task Team_Create_Renders_Npm_Team_Create()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmTeamCreateOptions("example-scope", "example-otp"));

        await Assert.That(commandLine.ToString()).IsEqualTo("npm team create example-scope example-otp");
    }

    [Test]
    public async Task Npx_C_Renders_Npx_Tool()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpxCOptions { Cmd = "example-command" });

        await Assert.That(commandLine.ToString()).IsEqualTo("npx -c example-command");
    }
}
