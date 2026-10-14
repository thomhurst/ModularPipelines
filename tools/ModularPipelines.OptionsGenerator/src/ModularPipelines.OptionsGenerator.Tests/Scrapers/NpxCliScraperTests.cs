using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NpxCliScraperTests
{
    [Test]
    public async Task Reuses_Npm_Exec_Syntax_Without_Emitting_Npm_Command_Parts()
    {
        var scraper = new NpxCliScraper(new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<NpxCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var root = commands.Single();
        await Assert.That(root.FullCommand).IsEqualTo("npx");
        await Assert.That(root.CommandParts).IsEmpty();
        await Assert.That(root.Options.Single(option => option.SwitchName == "--call").IsFlag).IsFalse();
        await Assert.That(root.Options.Single(option => option.SwitchName == "--call").ShortForm).IsEqualTo("-c");
        await Assert.That(root.Options.Single(option => option.SwitchName == "--package").AcceptsMultipleValues).IsTrue();
        await Assert.That(root.PositionalArguments.Any(argument => argument.Phase == CommandLinePhase.Passthrough)).IsTrue();
    }

    private sealed class HelpExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments == "--version" ? "11.11.0" : """
                    Run a command from a local or remote npm package
                    Usage:
                    npm exec -- <pkg>[@<version>] [args...]
                    npm exec --package=<pkg>[@<version>] -- <cmd> [args...]
                    npm exec -c '<cmd> [args...]'
                    npm exec --package=foo -c '<cmd> [args...]'

                    Options:
                    [--package <package-spec> [--package <package-spec> ...]] [-c|--call <call>]

                      --package
                        The package or packages to install.

                      -c|--call
                        Optional companion option for npm exec.
                    """,
            });
    }
}
