using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DockerCliCompatibilityTests
{
    [Test]
    public async Task SupportedAliases_IncludeAlias_WhenCanonicalCommandWasScraped()
    {
        var aliases = DockerCliCompatibility.GetSupportedCommandGroupAliases(
            [CreateCommand("buildx")]);

        var alias = aliases.Single();
        await Assert.That(alias.Alias).IsEqualTo("builder");
        await Assert.That(alias.CanonicalCommand).IsEqualTo("buildx");
    }

    [Test]
    public async Task SupportedAliases_ExcludeAlias_WhenCanonicalCommandWasNotScraped()
    {
        var aliases = DockerCliCompatibility.GetSupportedCommandGroupAliases(
            [CreateCommand("container")]);

        await Assert.That(aliases).IsEmpty();
    }

    [Test]
    [Arguments("exec")]
    [Arguments("run")]
    public async Task ComposeCommands_Preserve_Canonical_NoTty_Switch_Casing(string subcommand)
    {
        var helpText = $$"""
            Execute a command in a running container

            Usage: docker compose {{subcommand}} [OPTIONS] SERVICE COMMAND [ARGS...]

            Options:
              -T, --no-tty   Disable pseudo-TTY allocation
            """;
        var command = await new TestDockerCliScraper().Parse(
            ["docker", "compose", subcommand],
            helpText);

        var option = command!.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--no-TTY");
        await Assert.That(option.ShortForm).IsEqualTo("-T");
    }

    [Test]
    public async Task ComposeExec_Preserves_NoTty_WhenInstalledHelpOmitsIt()
    {
        const string helpText = """
            Execute a command in a running container

            Usage: docker compose exec [OPTIONS] SERVICE COMMAND [ARGS...]
            """;
        var command = await new TestDockerCliScraper().Parse(
            ["docker", "compose", "exec"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--no-TTY");
            await Assert.That(option.ShortForm).IsEqualTo("-T");
            await Assert.That(option.PropertyName).IsEqualTo("NoTty");
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.Description)
                .IsEqualTo("Disable pseudo-TTY allocation (default: auto-detected)");
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Switch_Normalization_Rejects_Distinct_Options_With_One_Canonical_Name()
    {
        const string helpText = """
            Usage: fake run [OPTIONS]

            Options:
              --current string   Current value
              --legacy string    Legacy value
            """;

        await Assert.That(() => new CollidingSwitchScraper().Parse(
                ["fake", "run"],
                helpText))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("maps both '--current' and '--legacy'");
    }

    [Test]
    public async Task Switch_Normalization_Treats_Source_Switch_Casing_As_Distinct()
    {
        const string helpText = """
            Usage: fake run [OPTIONS]

            Options:
              --current string   Current value
              --CURRENT string   Upper-case value
            """;

        await Assert.That(() => new CollidingSwitchScraper().Parse(
                ["fake", "run"],
                helpText))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("maps both '--current' and '--CURRENT'");
    }

    private static CliCommandDefinition CreateCommand(string commandGroup)
    {
        return new CliCommandDefinition
        {
            FullCommand = $"docker {commandGroup}",
            CommandParts = [commandGroup],
            ClassName = $"Docker{commandGroup}Options",
            ParentClassName = "DockerOptions",
            ToolNamespacePrefix = "Docker",
            Options = [],
        };
    }

    private sealed class TestDockerCliScraper : DockerCliScraper
    {
        public TestDockerCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<DockerCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class CollidingSwitchScraper : CobraCliScraper
    {
        public CollidingSwitchScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<CollidingSwitchScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override string NormalizeOptionSwitchName(
            string[] commandParts,
            string switchName) => "--canonical";

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
