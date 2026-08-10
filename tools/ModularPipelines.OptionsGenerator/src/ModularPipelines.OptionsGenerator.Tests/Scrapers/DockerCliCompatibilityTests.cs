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
    public async Task ComposeExec_Preserves_Canonical_NoTty_Switch_Casing()
    {
        const string helpText = """
            Execute a command in a running container

            Usage: docker compose exec [OPTIONS] SERVICE COMMAND [ARGS...]

            Options:
              -T, --no-tty   Disable pseudo-TTY allocation
            """;
        var command = await new TestDockerCliScraper().Parse(
            ["docker", "compose", "exec"],
            helpText);

        var option = command!.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--no-TTY");
        await Assert.That(option.ShortForm).IsEqualTo("-T");
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
}
