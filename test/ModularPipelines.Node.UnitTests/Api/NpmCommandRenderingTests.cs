using ModularPipelines.Context;
using ModularPipelines.Node.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Node.UnitTests.Api;

public class NpmCommandRenderingTests : TestBase
{
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

    [Test]
    public async Task Init_Does_Not_Render_Synopsis_Explanation()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmInitOptions("example-package"));

        await Assert.That(commandLine.ToString()).IsEqualTo("npm init example-package");
    }

    [Test]
    public async Task Org_Ls_Renders_Operands_As_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmOrgLsOptions("example-org")
        {
            Username = "example-user",
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("npm org ls example-org example-user");
    }

    [Test]
    public async Task Org_Rm_Renders_Operands_As_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmOrgRmOptions("example-org", "example-user"));

        await Assert.That(commandLine.ToString())
            .IsEqualTo("npm org rm example-org example-user");
    }

    [Test]
    public async Task Search_Does_Not_Add_A_Literal_Terms_Operand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmSearchOptions("example-term"));

        await Assert.That(commandLine.ToString()).IsEqualTo("npm search example-term");
    }

    [Test]
    public async Task Exec_Renders_Npm_Options_Before_The_Separator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmExecOptions("example-command", "example-argument")
        {
            Package = ["example-package"],
        });

        await Assert.That(commandLine.ToString()).IsEqualTo(
            "npm exec --package example-package -- example-command example-argument");
    }
}
