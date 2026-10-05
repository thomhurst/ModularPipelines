using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Podman.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Podman.UnitTests;

public class PodmanGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Root_Settings_Repeat_And_Precede_Subcommand()
    {
        var options = new PodmanPsOptions
        {
            Root = "/srv/containers",
            RuntimeFlag = ["debug", "log=/tmp/runtime.log"],
            Syslog = "false",
        };
        await AssertArguments(BuildArguments(options),
        [
            "--root=/srv/containers", "--runtime-flag=debug", "--runtime-flag=log=/tmp/runtime.log", "--syslog=false",
        ]);
        await Assert.That(await RenderCommand(options)).IsEqualTo(
            "podman --root=/srv/containers --runtime-flag=debug --runtime-flag=log=/tmp/runtime.log --syslog=false ps");
    }

    [Test]
    public async Task Remote_Bare_Flag_And_Aliases_Are_Preserved()
    {
        await Assert.That(await RenderCommand(new PodmanPsOptions
        {
            Connection = "builder",
            Remote = CliOptionValue.Bare,
        })).IsEqualTo("podman --connection=builder --remote ps");
        await Assert.That(typeof(PodmanOptions).GetProperty(nameof(PodmanOptions.Connection))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-c");
        await Assert.That(typeof(PodmanOptions).GetProperty(nameof(PodmanOptions.Remote))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-r");
    }

    [Test]
    public async Task Root_And_New_Connection_Identity_Have_Independent_Values()
    {
        var options = new PodmanSystemConnectionAddOptions("dev", "ssh://builder")
        {
            Identity = "active.key",
        };
        var localIdentity = typeof(PodmanSystemConnectionAddOptions)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Single(property => property.GetCustomAttribute<CliOptionAttribute>()?.Name == "--identity");
        localIdentity.SetValue(options, "new.key");
        await Assert.That(await RenderCommand(options)).IsEqualTo(
            "podman --identity=active.key system connection add --identity=new.key dev ssh://builder");
    }

    [Test]
    public async Task Compose_Settings_Remain_In_Provider_Scope()
    {
        await Assert.That(await RenderCommand(new PodmanComposeOptions
        {
            Connection = "builder",
            ProjectName = "demo",
        })).IsEqualTo("podman --connection=builder compose --project-name=demo");
        var globals = typeof(PodmanOptions)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        await Assert.That(globals).Count().IsEqualTo(30);
        await Assert.That(globals.Any(property => property.Name == "ProjectName")).IsFalse();
    }
}
