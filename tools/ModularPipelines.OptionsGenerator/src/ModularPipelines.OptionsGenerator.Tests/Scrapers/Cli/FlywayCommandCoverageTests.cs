using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class FlywayCommandCoverageTests
{
    [Test]
    public async Task FlywayScraper_LimitsParallelismForJavaProcesses()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestableFlywayCliScraper(
            new UnusedExecutor(),
            cache,
            NullLogger<FlywayCliScraper>.Instance);

        await Assert.That(scraper.Parallelism).IsEqualTo(2);
    }

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
                snapshot                 [enterprise] Produces a snapshot of the configured database
                                         A snapshot can also be generated from a schema model or empty source
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

        var evaluation = EvaluateCoverage(scraper, commands);

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

    [Test]
    public async Task FlywayCommunityCommandSurface_SatisfiesCommandCoveragePolicy()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new UnusedExecutor(),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var communityCommands = new[]
        {
            "auth",
            "baseline",
            "check",
            "clean",
            "diff",
            "diffText",
            "info",
            "init",
            "list-engines",
            "migrate",
            "repair",
            "snapshot",
            "validate",
        }.Select(Command).ToArray();
        var evaluation = EvaluateCoverage(scraper, communityCommands);

        var conditionallyAvailableCommands = scraper.CreateToolDefinition()
            .CommandCoverage
            .ConditionallyAvailableCommands
            .Select(command => command.Command);

        await Assert.That(evaluation.Violations).IsEmpty();
        await Assert.That(conditionallyAvailableCommands).IsEquivalentTo(
        [
            "flyway add",
            "flyway deploy",
            "flyway diffApply",
            "flyway generate",
            "flyway prepare",
            "flyway undo",
        ]);
    }

    [Test]
    public async Task FlywaySentinels_RejectIncompleteCommunityCommandSurface()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new UnusedExecutor(),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var incompleteCommands = new[]
        {
            "auth",
            "baseline",
            "check",
            "clean",
            "diff",
            "info",
            "init",
            "migrate",
            "repair",
            "snapshot",
            "validate",
        }.Select(Command).ToArray();
        var evaluation = EvaluateCoverage(scraper, incompleteCommands);

        await Assert.That(evaluation.Violations).Contains(
            violation => violation.Contains("configured minimum of 12", StringComparison.Ordinal));
        await Assert.That(evaluation.Violations).Contains(
            violation => violation.Contains("flyway diffText", StringComparison.Ordinal));
    }

    [Test]
    [Arguments("init")]
    [Arguments("list-engines")]
    public async Task PinnedFlywayCoverage_RejectsEitherRequiredCommandMissing(string missingCommand)
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(
            new UnusedExecutor(),
            cache,
            NullLogger<FlywayCliScraper>.Instance);
        var completeCommands = new[]
        {
            "auth",
            "baseline",
            "check",
            "clean",
            "diff",
            "diffText",
            "info",
            "init",
            "list-engines",
            "migrate",
            "repair",
            "snapshot",
            "validate",
        };
        var commands = completeCommands
            .Where(command => !command.Equals(missingCommand, StringComparison.Ordinal))
            .Select(Command)
            .ToArray();

        var evaluation = EvaluateCoverage(scraper, commands);

        await Assert.That(evaluation.Violations).Contains(
            violation => violation.Contains($"flyway {missingCommand}", StringComparison.Ordinal));
    }

    private static CommandCoverageEvaluation EvaluateCoverage(
        FlywayCliScraper scraper,
        IEnumerable<CliCommandDefinition> commands)
    {
        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-flyway-coverage-tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);

        try
        {
            var tool = scraper.CreateToolDefinition() with { Commands = commands.ToArray() };
            return CommandCoverageGuard.Evaluate(
                tool,
                outputDirectory,
                approveShrinkage: false);
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

    private sealed class TestableFlywayCliScraper(
        ICliCommandExecutor executor,
        IHelpTextCache helpCache,
        ILogger<FlywayCliScraper> logger)
        : FlywayCliScraper(executor, helpCache, logger)
    {
        public int Parallelism => MaxParallelism;
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
