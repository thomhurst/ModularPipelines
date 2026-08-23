using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KubectlCliScraperTests
{
    [Test]
    public async Task Uses_Version_Subcommand_For_Availability_And_Version()
    {
        var executor = new RecordingExecutor();
        var scraper = new KubectlCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KubectlCliScraper>.Instance);

        var isAvailable = await scraper.IsAvailableAsync();
        var version = await scraper.GetVersionAsync();

        await Assert.That(isAvailable).IsTrue();
        await Assert.That(version).IsEqualTo("Client Version: v1.35.0");
        await Assert.That(executor.AvailabilityArguments).IsEquivalentTo(["version --client"]);
        await Assert.That(executor.Arguments).IsEquivalentTo(["version --client"]);
    }

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public List<string> AvailabilityArguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments.Add(arguments);
            var success = arguments == "version --client";
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? "Client Version: v1.35.0" : string.Empty,
                StandardError = success ? string.Empty : "error: unknown flag: --version",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> IsAvailableAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default)
        {
            AvailabilityArguments.Add(arguments);
            return Task.FromResult(arguments == "version --client");
        }
    }
}
