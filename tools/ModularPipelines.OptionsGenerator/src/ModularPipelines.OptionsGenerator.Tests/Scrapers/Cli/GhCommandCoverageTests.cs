using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GhCommandCoverageTests
{
    [Test]
    [Arguments("stack", true, false)]
    [Arguments("stack init", false, false)]
    [Arguments("stacked", false, false)]
    [Arguments("issue", false, false)]
    [Arguments("stack", false, true)]
    [Arguments("stack", false, false, true)]
    public async Task Only_Extension_Absence_Is_Allowed_At_The_Same_Version(
        string removedCommand, bool allowed, bool unavailable, bool keepDescendants = false)
    {
        var scraper = CreateScraper(new ExtensionExecutor());
        var tool = (await scraper.CreateToolDefinitionAsync()) with
        {
            ToolVersion = "gh version 2.100.0",
            Commands = [Command("stack"), Command("stack init"), Command("stacked"), Command("issue")],
        };
        tool = tool with { CommandCoverage = tool.CommandCoverage with { SentinelCommands = ["gh stack init"] } };
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"gh-coverage-{Guid.NewGuid():N}");
        Directory.CreateDirectory(outputDirectory);
        try
        {
            var baseline = CommandCoverageGuard.Evaluate(tool, outputDirectory, approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(tool with
            {
                Commands = [.. tool.Commands.Where(command => command.FullCommand != $"gh {removedCommand}"
                    && (keepDescendants || !command.FullCommand.StartsWith($"gh {removedCommand} ", StringComparison.Ordinal)))],
            }, outputDirectory, approveShrinkage: false,
                unavailableHelpPaths: unavailable ? ["gh stack"] : null);

            if (allowed)
            {
                await Assert.That(current.Violations).IsEmpty();
            }
            else
            {
                await Assert.That(current.Violations).IsNotEmpty();
            }
            string[] expectedRemovals = (unavailable, removedCommand) switch
            {
                (true, _) => [],
                (false, "stack") when !keepDescendants => ["gh stack", "gh stack init"],
                _ => [$"gh {removedCommand}"],
            };
            await Assert.That(current.RemovedCommands).IsEquivalentTo(expectedRemovals);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    [Arguments("gh stack\tgithub/gh-stack\tv0.1.0\n", false)]
    [Arguments("gh stack\t\t\n", false)]
    [Arguments("gh other\tuser/gh-other\tv1\ngh stack\tcustom/gh-stack\tv2\n", false)]
    [Arguments("gh stacked\tuser/gh-stacked\tv1\n", true)]
    [Arguments("gh other\t\t\r\n", true)]
    [Arguments("", true)]
    public async Task Missing_Whole_Subtree_Requires_Independent_Absence_Evidence(string extensions, bool allowed)
    {
        var executor = new ExtensionExecutor { Output = extensions };
        var scraper = CreateScraper(executor);
        var metadata = await scraper.CreateToolDefinitionAsync();
        var tool = metadata with
        {
            ToolVersion = "gh version 2.100.0",
            Commands = [Command("stack"), Command("stack init"), Command("issue")],
            CommandCoverage = metadata.CommandCoverage with { SentinelCommands = ["gh stack init"] },
        };
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"gh-coverage-{Guid.NewGuid():N}");
        Directory.CreateDirectory(outputDirectory);
        try
        {
            await CommandCoverageGuard.WriteManifestAsync(
                CommandCoverageGuard.Evaluate(tool, outputDirectory, approveShrinkage: false), CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(tool with { Commands = [Command("issue")] },
                outputDirectory, approveShrinkage: false);

            await Assert.That(current.Violations.Count == 0).IsEqualTo(allowed);
            await Assert.That(executor.Calls).IsEquivalentTo(["gh extension list"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    [Arguments("", "failed", 1, "")]
    [Arguments("", "", 0, "timeout")]
    [Arguments("", "", 0, "circuit")]
    [Arguments("", "", 0, "launch")]
    [Arguments("gh other\tuser/gh-other\tv1", "incomplete listing", 0, "")]
    [Arguments("unexpected output", "", 0, "")]
    [Arguments("gh stack", "", 0, "")]
    [Arguments("gh other\tuser/gh-other\tv1\nbad row", "", 0, "")]
    public async Task Unreliable_Inventory_Fails_Generation(string output, string error, int exitCode, string failure)
    {
        var scraper = CreateScraper(new ExtensionExecutor
        {
            Output = output,
            Error = error,
            ExitCode = exitCode,
            Failure = failure,
        });

        await Assert.That(async () => { await scraper.CreateToolDefinitionAsync(); }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Synchronous_Metadata_Does_Not_Exempt_Unverified_Extensions()
    {
        var executor = new ExtensionExecutor();
        var scraper = CreateScraper(executor);

        await Assert.That(scraper.CreateToolDefinition().CommandCoverage.ConditionallyAvailableCommands).IsEmpty();
        await Assert.That(executor.Calls).IsEmpty();
    }

    private static GhCliScraper CreateScraper(ICliCommandExecutor executor) => new(
        executor,
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<GhCliScraper>.Instance);

    private sealed class ExtensionExecutor : ICliCommandExecutor
    {
        public string Output { get; init; } = "";
        public string Error { get; init; } = "";
        public int ExitCode { get; init; }
        public string Failure { get; init; } = "";
        public List<string> Calls { get; } = [];

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Calls.Add($"{command} {arguments}");
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = Output,
                StandardError = Error,
                ExitCode = ExitCode,
                TimedOut = Failure == "timeout",
                CircuitOpen = Failure == "circuit",
                ExecutionFailed = Failure == "launch",
            });
        }
    }

    private static CliCommandDefinition Command(string name) => new()
    {
        FullCommand = $"gh {name}",
        CommandParts = name.Split(' '),
        ClassName = $"Gh{name}Options",
        ParentClassName = "GhOptions",
        ToolNamespacePrefix = "Gh",
        Options = [],
    };
}
