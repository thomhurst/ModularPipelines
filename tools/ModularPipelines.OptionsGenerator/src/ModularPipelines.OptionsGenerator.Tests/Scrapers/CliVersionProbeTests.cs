using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CliVersionProbeTests
{
    [Test]
    public async Task ArgoCd_Uses_Client_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "argocd",
            "version --client",
            (executor, cache) => new ArgoCdCliScraper(
                executor,
                cache,
                NullLogger<ArgoCdCliScraper>.Instance));

    [Test]
    public async Task Eksctl_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "eksctl",
            "version",
            (executor, cache) => new EksctlCliScraper(
                executor,
                cache,
                NullLogger<EksctlCliScraper>.Instance));

    [Test]
    public async Task Cosign_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "cosign",
            "version",
            (executor, cache) => new CosignCliScraper(
                executor,
                cache,
                NullLogger<CosignCliScraper>.Instance));

    [Test]
    public async Task Kustomize_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "kustomize",
            "version",
            (executor, cache) => new KustomizeCliScraper(
                executor,
                cache,
                NullLogger<KustomizeCliScraper>.Instance));

    [Test]
    public async Task Go_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "go",
            "version",
            (executor, cache) => new GoCliScraper(
                executor,
                cache,
                NullLogger<GoCliScraper>.Instance));

    private static async Task AssertVersionProbeAsync(
        string expectedCommand,
        string expectedArguments,
        Func<ICliCommandExecutor, IHelpTextCache, ICliScraper> createScraper)
    {
        var executor = new RecordingExecutor(expectedCommand, expectedArguments);
        var scraper = createScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance));

        var isAvailable = await scraper.IsAvailableAsync();

        using (Assert.Multiple())
        {
            await Assert.That(isAvailable).IsTrue();
            await Assert.That(executor.InvocationCount).IsEqualTo(1);
        }
    }

    private sealed class RecordingExecutor(string expectedCommand, string expectedArguments)
        : ICliCommandExecutor
    {
        public int InvocationCount { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            InvocationCount++;
            var success = command.Equals(expectedCommand, StringComparison.Ordinal)
                          && arguments.Equals(expectedArguments, StringComparison.Ordinal);
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? "version" : string.Empty,
                StandardError = success ? string.Empty : "unexpected probe",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public async Task<bool> IsAvailableAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default) =>
            (await ExecuteAsync(command, arguments, cancellationToken)).Success;
    }
}
