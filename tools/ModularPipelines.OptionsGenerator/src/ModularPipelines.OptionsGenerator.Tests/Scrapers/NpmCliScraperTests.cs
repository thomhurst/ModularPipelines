using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NpmCliScraperTests
{
    [Test]
    public async Task Discovers_Wrapped_Root_Commands()
    {
        var scraper = CreateScraper();

        await Assert.That(scraper.GetSubcommands("""
            npm <command>

            All commands:

                access, adduser, audit, bugs, cache, ci, completion,
                config, dedupe, dist-tag, install, package-name

            Specify configs in the ini-formatted file:
            """))
            .IsEquivalentTo([
                "access",
                "adduser",
                "audit",
                "bugs",
                "cache",
                "ci",
                "completion",
                "config",
                "dedupe",
                "dist-tag",
                "install",
                "package-name",
            ]);
    }

    [Test]
    public async Task Parses_Npm_Options_And_Positionals()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "install"],
            """
            Install a package

            Usage:
            npm install [<package-spec> ...]

            Options:
            [-S|--save] [-g|--global] [--omit <type> [--omit <type> ...]]

              -S|--save
                Save installed packages.

              -g|--global
                Operates in global mode.

              --omit
                Dependency types to omit. This option may be specified multiple times.
            """);

        await Assert.That(command!.ClassName).IsEqualTo("NpmInstallOptions");
        await Assert.That(command.PositionalArguments.Single().CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--save").ShortForm)
            .IsEqualTo("-S");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--global").IsFlag)
            .IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--omit").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Keeps_Synopsis_Explanation_Out_Of_Command_Parts()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "init"],
            """
            Create a package.json file

            Usage:
            npm init <package-spec> (same as `npx create-<package-spec>`)
            npm init <@scope> (same as `npx <@scope>/create`)
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["init"]);
        await Assert.That(command.PositionalArguments).Count().IsEqualTo(1);
        await Assert.That(command.PositionalArguments[0].PropertyName).IsEqualTo("Value");
        await Assert.That(command.PositionalArguments[0].CSharpType).IsEqualTo("string?");
        await Assert.That(command.PositionalArguments[0].IsRequired).IsFalse();
    }

    [Test]
    public async Task Search_Does_Not_Treat_The_Operand_As_A_Subcommand()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "search"],
            """
            Search for packages

            Usage:
            npm search <search term> [<search term> ...]
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["search"]);
        await Assert.That(command.PositionalArguments).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Exec_Attaches_The_Separator_To_The_Command_Operand()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "exec"],
            """
            Run a command

            Usage:
            npm exec --package=<pkg> -- <cmd> [args...]

            Options:
            [--package <package-spec>]
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["exec"]);
        var operand = command.PositionalArguments.First();
        await Assert.That(operand.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(operand.PrependOptionTerminator).IsTrue();
    }

    private static TestNpmCliScraper CreateScraper() => new();

    private sealed class TestNpmCliScraper : NpmCliScraper
    {
        public TestNpmCliScraper()
            : base(
                new UnusedExecutor(),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<NpmCliScraper>.Instance)
        {
        }

        public string[] GetSubcommands(string helpText) => [.. ExtractSubcommands(helpText)];

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class UnusedExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();
    }
}
