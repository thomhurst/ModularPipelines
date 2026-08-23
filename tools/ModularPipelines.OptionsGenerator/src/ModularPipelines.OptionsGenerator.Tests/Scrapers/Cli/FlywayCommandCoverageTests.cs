using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class FlywayCommandCoverageTests
{
    [Test]
    public async Task FlywayVersion_ExtractsEditionVersionFromNoisyOutput()
    {
        const string versionOutput = """
            WARNING: A more recent version of Flyway is available. Find out more about Flyway 13.3.0.
            Flyway Community Edition 10.20.1 by Redgate

            Plugin Name       | Version
            Redgate Compare   | 1.24.2.24235
            """;
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new FlywayExecutor(versionOutput: versionOutput),
            cache,
            NullLogger<FlywayCliScraper>.Instance);

        var version = await scraper.GetVersionAsync();

        await Assert.That(version).IsEqualTo("10.20.1");
    }

    [Test]
    public async Task LegacyConfigurationOnlyHelp_PreservesCommandAndOptions()
    {
        const string rootHelp = """
            Usage
                flyway [options] [command]

            Commands
                migrate  Migrates the database
            """;
        const string commandHelp = """
            Configuration
            -------------
            -url=  : Jdbc url
            """;
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new FlywayExecutor(rootHelp, commandHelp),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var migrate = commands.Single();
        await Assert.That(migrate.FullCommand).IsEqualTo("flyway migrate");
        await Assert.That(migrate.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["-url"]);
    }

    [Test]
    public async Task RewordedSectionHeading_TerminatesCommandTable()
    {
        const string rootHelp = """
            Usage
                flyway [options] [command]

            Commands
                migrate                  Migrates the database

            Runtime settings
                url                      Jdbc url
            """;
        const string commandHelp = """
            Description:
                Flyway command
            """;
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new FlywayExecutor(rootHelp, commandHelp),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["flyway migrate"]);
    }

    [Test]
    public async Task PinnedFlywayHelp_SatisfiesCommandCoveragePolicy()
    {
        const string rootHelp = """
            Usage
                flyway [options] [command]

            Commands
                help                     Print this usage info and exit
                auth                     Authenticates Flyway with Redgate licensing
                migrate                  Migrates the database
                clean                    Drops all objects in the configured schemas
                info                     Prints migration information
                validate                 Validates applied migrations
                baseline                 Baselines an existing database
                repair                   Repairs the schema history table
                check                    Produces migration reports
                version, -v, --version   Print the Flyway version and edition
                list-engines             Lists supported database engines
                diff (preview)           Compares two comparison sources
                diffText (preview)       Shows object differences
                snapshot                 Produces a database snapshot
                diffApply (preview)      Applies changes from flyway diff
                generate (preview)       Generates a migration script
                add (preview)            Creates an empty migration script
                undo                     Undoes the latest migration
                deploy                   Deploys a script to an environment
                prepare                  Writes a deployment script
                init                     Initializes a Flyway project

            Configuration parameters (Format: -key=value)
                url                      Jdbc url
            """;
        const string commandHelp = """
            Description:
                Flyway command
            """;
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new FlywayExecutor(rootHelp, commandHelp),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-flyway-coverage-tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);

        try
        {
            var tool = scraper.CreateToolDefinition() with { Commands = commands };
            var evaluation = CommandCoverageGuard.Evaluate(
                tool,
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(
            [
                "flyway add",
                "flyway auth",
                "flyway baseline",
                "flyway check",
                "flyway clean",
                "flyway deploy",
                "flyway diff",
                "flyway diffApply",
                "flyway diffText",
                "flyway generate",
                "flyway info",
                "flyway init",
                "flyway list-engines",
                "flyway migrate",
                "flyway prepare",
                "flyway repair",
                "flyway snapshot",
                "flyway undo",
                "flyway validate",
            ]);
            await Assert.That(evaluation.Violations).IsEmpty();
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task FlywaySentinels_RejectTheKnownPartialCommandSurface()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new UnusedExecutor(),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var partialCommands = new[]
        {
            "baseline",
            "check",
            "clean",
            "deploy",
            "info",
            "init",
            "listEngines",
            "migrate",
            "prepare",
            "repair",
            "snapshot",
            "validate",
        }.Select(Command).ToArray();
        var tool = scraper.CreateToolDefinition() with { Commands = partialCommands };
        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-flyway-coverage-tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);

        try
        {
            var evaluation = CommandCoverageGuard.Evaluate(
                tool,
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(evaluation.Violations).Contains(
                violation => violation.Contains("flyway auth", StringComparison.Ordinal));
            await Assert.That(evaluation.Violations).Contains(
                violation => violation.Contains("configured minimum of 19", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    private static CliCommandDefinition Command(string command) => new()
    {
        FullCommand = $"flyway {command}",
        CommandParts = [command],
        ClassName = $"Flyway{command}Options",
        ParentClassName = "FlywayOptions",
        ToolNamespacePrefix = "Flyway",
        Options = [],
    };

    private sealed class UnusedExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    private sealed class FlywayExecutor(
        string? rootHelp = null,
        string? commandHelp = null,
        string? versionOutput = null) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments switch
                {
                    "-v" => versionOutput ?? string.Empty,
                    "--help" => rootHelp ?? string.Empty,
                    _ => commandHelp ?? string.Empty,
                },
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
