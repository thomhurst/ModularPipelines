using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class FlywayCommandCoverageTests
{
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
}
