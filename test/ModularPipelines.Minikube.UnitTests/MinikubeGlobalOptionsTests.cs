using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Minikube.Options;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Minikube.UnitTests;

public class MinikubeGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments("true", "minikube --logtostderr=true --profile=ci --v=0 config view")]
    [Arguments("false", "minikube --logtostderr=false --profile=ci --v=0 config view")]
    [Arguments(null, "minikube --profile=ci --v=0 config view")]
    public async Task Persistent_Settings_Precede_Nested_Command(string? enabled, string expected)
    {
        var command = await RenderCommand(new MinikubeConfigViewOptions
        {
            Logtostderr = enabled,
            Profile = "ci",
            V = 0,
        });
        await Assert.That(command).IsEqualTo(expected);
    }

    [Test]
    public async Task Status_Template_Preserves_Multiline_Value_And_Local_Scope()
    {
        const string template = "{{.Name}}\nhost: {{.Host}}";
        await AssertArguments(BuildArguments(new MinikubeStatusOptions
        {
            Profile = "ci",
            Format = template,
        }), ["--profile=ci", $"--format={template}"]);
        await Assert.That(await RenderCommand(new MinikubeStatusOptions { Profile = "ci", Format = "template" }))
            .IsEqualTo("minikube --profile=ci status --format=template");
        await Assert.That(typeof(MinikubeStatusOptions).GetProperty(nameof(MinikubeStatusOptions.Format))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-f");
        await Assert.That(typeof(MinikubeOptions).GetProperty("Format")).IsNull();
    }

    [Test]
    public async Task Config_View_Template_Preserves_Multiline_Value_And_Nested_Command()
    {
        const string template = "{{.ConfigKey}}:\n{{.ConfigValue}}";
        await AssertArguments(BuildArguments(new MinikubeConfigViewOptions
        {
            Profile = "ci",
            Format = template,
        }), ["--profile=ci", $"--format={template}"]);
        await Assert.That(await RenderCommand(new MinikubeConfigViewOptions { Profile = "ci", Format = "template" }))
            .IsEqualTo("minikube --profile=ci config view --format=template");
        await Assert.That(typeof(MinikubeConfigViewOptions).GetProperty(nameof(MinikubeConfigViewOptions.Format))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsNull();
    }

    [Test]
    public async Task Bare_Boolean_Does_Not_Consume_Command()
    {
        var command = await RenderCommand(new MinikubeConfigViewOptions { Rootless = CliOptionValue.Bare });
        await Assert.That(command).IsEqualTo("minikube --rootless config view");
    }

    [Test]
    public async Task Values_Preserve_Whitespace_Severity_And_Unsigned_Range()
    {
        await AssertArguments(BuildArguments(new MinikubeConfigViewOptions
        {
            Alsologtostderrthreshold = "ERROR",
            LogDir = "log directory",
            LogFileMaxSize = ulong.MaxValue,
            Profile = "build profile",
            Stderrthreshold = "WARNING",
            User = "build user",
        }),
        [
            "--alsologtostderrthreshold=ERROR",
            "--log_dir=log directory",
            "--log_file_max_size=18446744073709551615",
            "--profile=build profile",
            "--stderrthreshold=WARNING",
            "--user=build user",
        ]);
    }

    [Test]
    [Arguments("Profile", "-p")]
    [Arguments("Bootstrapper", "-b")]
    [Arguments("V", "-v")]
    public async Task Persistent_Aliases_Are_Preserved(string property, string alias)
    {
        await Assert.That(typeof(MinikubeOptions).GetProperty(property)!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo(alias);
    }

    [Test]
    public async Task Every_Command_Inherits_Settings_Exactly_Once_Without_Promoting_Local_Options()
    {
        string[] expected = ["AddDirHeader", "Alsologtostderr", "Alsologtostderrthreshold", "Bootstrapper",
            "LegacyStderrThresholdBehavior", "LogBacktraceAt", "LogDir", "LogFile", "LogFileMaxSize",
            "Logtostderr", "OneOutput", "Profile", "Rootless", "SkipAudit", "SkipHeaders", "SkipLogHeaders",
            "Stderrthreshold", "User", "V", "Vmodule"];
        await Assert.That(typeof(MinikubeOptions)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(property => property.Name)).IsEquivalentTo(expected);
        var commands = typeof(MinikubeOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(MinikubeOptions).IsAssignableFrom(type)).ToArray();
        await Assert.That(commands).Count().IsEqualTo(48);
        foreach (var command in commands)
        {
            foreach (var name in expected)
            {
                await Assert.That(command.GetProperties().Single(property => property.Name == name).DeclaringType)
                    .IsEqualTo(typeof(MinikubeOptions));
            }
        }

        await Assert.That(typeof(MinikubeStartOptions).GetProperty("Driver")!.DeclaringType)
            .IsEqualTo(typeof(MinikubeStartOptions));
        await Assert.That(typeof(MinikubeStatusOptions).GetProperty("Output")!.DeclaringType)
            .IsEqualTo(typeof(MinikubeStatusOptions));
    }
}
