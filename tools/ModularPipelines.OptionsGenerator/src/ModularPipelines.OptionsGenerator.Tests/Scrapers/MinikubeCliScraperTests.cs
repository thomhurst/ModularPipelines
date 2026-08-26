using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class MinikubeCliScraperTests
{
    [Test]
    public async Task Reads_Version_From_Minikube_Version_Command()
    {
        var executor = new RecordingExecutor();
        var scraper = new MinikubeCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<MinikubeCliScraper>.Instance);

        var version = await scraper.GetVersionAsync();

        using (Assert.Multiple())
        {
            await Assert.That(executor.Arguments).IsEqualTo("version --short");
            await Assert.That(version).IsEqualTo("v1.38.1");
        }
    }

    [Test]
    public async Task Rejects_Version_Command_Errors()
    {
        var executor = new RecordingExecutor
        {
            Result = new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = "Error: unknown flag: --version",
                ExitCode = 1,
            },
        };
        var scraper = new MinikubeCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<MinikubeCliScraper>.Instance);

        var version = await scraper.GetVersionAsync();

        await Assert.That(version).IsNull();
    }

    [Test]
    [Arguments("addons", "list")]
    [Arguments("config", "view")]
    public async Task Shared_Traversal_Removes_Subcommand_Placeholders(string group, string child)
    {
        var executor = new RecordingExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = $"""
                Usage:
                  minikube COMMAND

                Available Commands:
                  {group}    Manage {group}
                """,
            [$"{group} --help"] = $"""
                Usage:
                  minikube {group} SUBCOMMAND [flags]

                Available Commands:
                  {child}    Execute {child}

                Flags:
                  --output string    Output format
                """,
            [$"{group} {child} --help"] = $"""
                Usage:
                  minikube {group} {child} [flags]

                Flags:
                  --output string    Output format
                """,
        });
        var scraper = new MinikubeCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<MinikubeCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        using (Assert.Multiple())
        {
            await Assert.That(commands.Select(command => command.FullCommand))
                .IsEquivalentTo([$"minikube {group}", $"minikube {group} {child}"]);
            await Assert.That(commands.Single(command => command.FullCommand == $"minikube {group}")
                    .PositionalArguments)
                .IsEmpty();
        }
    }

    private sealed class RecordingExecutor(
        IReadOnlyDictionary<string, string>? responses = null) : ICliCommandExecutor
    {
        public string? Arguments { get; private set; }

        public CliCommandResult Result { get; init; } = new()
        {
            StandardOutput = "v1.38.1",
            StandardError = string.Empty,
            ExitCode = 0,
        };

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments = arguments;
            if (responses is not null)
            {
                if (!responses.TryGetValue(arguments, out var response))
                {
                    throw new InvalidOperationException($"Unexpected invocation: {command} {arguments}");
                }

                return Task.FromResult(new CliCommandResult
                {
                    StandardOutput = response,
                    StandardError = string.Empty,
                    ExitCode = 0,
                });
            }

            return Task.FromResult(Result);
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
