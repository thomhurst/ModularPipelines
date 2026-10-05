using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Secrets;
using ModularPipelines.Syft.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Syft.UnitTests;

public class SyftGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Repeated_Settings_Precede_Nested_Command_And_Keep_Argument_Boundaries()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new SyftConfigLocationsOptions
        {
            Config = ["team config.yaml", "override.yaml"],
            Profile = ["ci", "security"],
            Verbose = 2,
            All = true,
        });

        await Assert.That(string.Join("|", command.Arguments)).IsEqualTo(
            "--config=team config.yaml|--config=override.yaml|--profile=ci|--profile=security|--verbose=2|config|locations|--all");
    }

    [Test]
    public async Task Quiet_Precedes_Command_While_Local_Flags_Follow()
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new SyftConfigOptions { Quiet = true, Load = true }).ToString())
            .IsEqualTo("syft --quiet config --load");
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Unset_Settings_And_False_Quiet_Are_Omitted(bool? quiet)
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new SyftConfigLocationsOptions { Quiet = quiet }).ToString())
            .IsEqualTo("syft config locations");
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    public async Task Verbosity_Keeps_Explicit_Count(int count)
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new SyftConfigLocationsOptions { Verbose = count }).ToString())
            .IsEqualTo($"syft --verbose={count} config locations");
    }

    [Test]
    public async Task Every_Command_Inherits_Exactly_One_Copy()
    {
        var commands = typeof(SyftOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(SyftOptions))).ToArray();
        await Assert.That(commands).IsNotEmpty();
        foreach (var command in commands)
        {
            foreach (var name in new[] { nameof(SyftOptions.Config), nameof(SyftOptions.Profile), nameof(SyftOptions.Quiet), nameof(SyftOptions.Verbose) })
            {
                var properties = command.GetProperties().Where(property => property.Name == name).ToArray();
                await Assert.That(properties.Length).IsEqualTo(1);
                await Assert.That(properties.Single().DeclaringType).IsEqualTo(typeof(SyftOptions));
            }
        }

        await Assert.That(typeof(SyftOptions).GetProperty("Output")).IsNull();
        await Assert.That(typeof(SyftOptions).GetProperty("Password")).IsNull();
        await Assert.That(typeof(SyftConfigLocationsOptions).GetProperty("Load")).IsNull();
    }

    [Test]
    public async Task Global_Aliases_Keep_Their_Original_Spelling()
    {
        await Assert.That(typeof(SyftOptions).GetProperty(nameof(SyftOptions.Config))!.GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-c");
        await Assert.That(typeof(SyftOptions).GetProperty(nameof(SyftOptions.Verbose))!.GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-v");
        await Assert.That(typeof(SyftOptions).GetProperty(nameof(SyftOptions.Quiet))!.GetCustomAttribute<CliFlagAttribute>()!.ShortForm).IsEqualTo("-q");
    }

    [Test]
    public async Task Login_Password_Remains_Local_And_Masked()
    {
        var options = new SyftLoginOptions { Config = ["team.yaml"], Password = "syft-test-password", Username = "user", Server = "registry.example" };
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(options);
        await Assert.That(command.Arguments.Take(2)).IsEquivalentTo(["--config=team.yaml", "login"]);
        await Assert.That(command.Arguments).Contains("--password=syft-test-password");
        var obfuscator = await GetService<ISecretObfuscator>();
        var masked = obfuscator.Obfuscate(command.ToString(), options);
        await Assert.That(masked).DoesNotContain("syft-test-password");
        await Assert.That(masked).Contains("team.yaml");
    }
}
