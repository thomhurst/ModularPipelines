using ModularPipelines.Context;
using ModularPipelines.Node.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Node.UnitTests.Api;

public class NpmCommandRenderingTests : TestBase
{
    [Test]
    [Arguments("ls")]
    [Arguments("pack")]
    [Arguments("publish")]
    [Arguments("run")]
    public async Task Current_Project_Commands_Render_Without_Operands(string verb)
    {
        var builder = await GetService<ICommandLineBuilder>();
        NpmOptions options = verb switch
        {
            "ls" => new NpmLsOptions(),
            "pack" => new NpmPackOptions(),
            "publish" => new NpmPublishOptions(),
            _ => new NpmRunOptions(),
        };
        await Assert.That(builder.Build(options).ToString()).IsEqualTo($"npm {verb}");
    }

    [Test]
    [Arguments("run")]
    [Arguments("start")]
    [Arguments("stop")]
    [Arguments("test")]
    [Arguments("restart")]
    public async Task Script_Arguments_Render_As_Separate_Tokens(string verb)
    {
        var builder = await GetService<ICommandLineBuilder>();
        string[] arguments = ["--output", "dist", "--watch"];
        NpmOptions options = verb switch
        {
            "run" => new NpmRunOptions { Command = "build", Args = arguments },
            "start" => new NpmStartOptions { Args = arguments },
            "stop" => new NpmStopOptions { Args = arguments },
            "test" => new NpmTestOptions { Args = arguments },
            _ => new NpmRestartOptions { Args = arguments },
        };
        string[] expected = verb == "run"
            ? [verb, "build", "--", .. arguments]
            : [verb, "--", .. arguments];
        await Assert.That(builder.Build(options).Arguments.SequenceEqual(expected)).IsTrue();
    }

    [Test]
    public async Task Package_Filters_And_Pack_Specifications_Remain_Separate_Operands()
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new NpmLsOptions { PackageSpec = ["one", "two"] }).Arguments
            .SequenceEqual(["ls", "one", "two"])).IsTrue();
        await Assert.That(builder.Build(new NpmPackOptions { PackageSpec = ["one", "two"] }).Arguments
            .SequenceEqual(["pack", "one", "two"])).IsTrue();
        await Assert.That(builder.Build(new NpmPublishOptions { PackageSpec = "./package.tgz" }).Arguments
            .SequenceEqual(["publish", "./package.tgz"])).IsTrue();
    }

    [Test]
    public async Task Version_Preid_Renders_A_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var commandLine = builder.Build(new NpmVersionOptions { Newversion = "prerelease", Preid = "beta" });
        await Assert.That(commandLine.ToString()).IsEqualTo("npm version prerelease --preid beta");
    }

    [Test]
    public async Task Get_And_Set_Do_Not_Render_Help_Notes()
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new NpmGetOptions { Key = ["registry"] }).ToString())
            .IsEqualTo("npm get registry");
        await Assert.That(builder.Build(new NpmSetOptions(["registry=https://registry.npmjs.org/"])).ToString())
            .IsEqualTo("npm set registry=https://registry.npmjs.org/");
    }

    [Test]
    public async Task Npx_Renders_Packages_Before_Passthrough_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var commandLine = builder.Build(new NpxExecuteOptions
        {
            Package = ["typescript"],
            Pkg = "tsc",
            Args = ["--noEmit"],
        });
        await Assert.That(commandLine.ToString()).IsEqualTo("npx --package typescript -- tsc --noEmit");
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

        var commandLine = builder.Build(new NpmTeamCreateOptions("example-scope:example-team") { Otp = "example-otp" });

        await Assert.That(commandLine.ToString()).IsEqualTo("npm team create example-scope:example-team --otp example-otp");
    }

    [Test]
    public async Task Npx_Call_Renders_Npx_Tool()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpxExecuteOptions { Call = "example-command" });

        await Assert.That(commandLine.ToString()).IsEqualTo("npx --call example-command");
    }

    [Test]
    public async Task Init_Does_Not_Render_Synopsis_Explanation()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmInitOptions { Value = "example-package" });

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

        var commandLine = builder.Build(new NpmSearchOptions(["example-term"]));

        await Assert.That(commandLine.ToString()).IsEqualTo("npm search example-term");
    }

    [Test]
    public async Task Exec_Renders_Npm_Options_Before_The_Separator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new NpmExecOptions
        {
            Package = ["example-package"],
            Pkg = "example-command",
            Args = ["example-argument"],
        });

        await Assert.That(commandLine.ToString()).IsEqualTo(
            "npm exec --package example-package -- example-command example-argument");
    }
}
