using Microsoft.Extensions.Logging.Abstractions;
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

    private sealed class RecordingExecutor : ICliCommandExecutor
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
            return Task.FromResult(Result);
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
