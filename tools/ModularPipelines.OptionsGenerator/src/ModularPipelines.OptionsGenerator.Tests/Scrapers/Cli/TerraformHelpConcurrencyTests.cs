using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class TerraformHelpConcurrencyTests
{
    private static readonly string?[] ExpectedStacksHelp = ["Usage: stacks list -help", "Usage: stacks deployment-run -help"];

    [Test]
    [Timeout(30_000)]
    public async Task Stacks_Help_Avoids_Concurrent_Manifest_Reads_Across_Scrapers(CancellationToken cancellationToken)
    {
        var executor = new ManifestCacheExecutor();
        var first = new TestScraper(executor).ReadHelp(["terraform", "stacks", "list"], cancellationToken);
        try
        {
            await executor.FirstStarted.Task.WaitAsync(cancellationToken);
            var second = new TestScraper(executor).ReadHelp(["terraform", "stacks", "deployment-run"], cancellationToken);
            executor.ReleaseFirst.TrySetResult();

            var help = await Task.WhenAll(first, second);
            await Assert.That(help).IsEquivalentTo(ExpectedStacksHelp);
        }
        finally
        {
            executor.ReleaseFirst.TrySetResult();
            await first;
        }
    }

    [Test]
    [Timeout(30_000)]
    public async Task Core_Help_Does_Not_Wait_For_Stacks_Manifest(CancellationToken cancellationToken)
    {
        var executor = new ManifestCacheExecutor();
        var scraper = new TestScraper(executor);
        var first = scraper.ReadHelp(["terraform", "stacks", "list"], cancellationToken);
        try
        {
            await executor.FirstStarted.Task.WaitAsync(cancellationToken);
            var core = await scraper.ReadHelp(["terraform", "plan"], cancellationToken);
            await Assert.That(core).IsEqualTo("Usage: plan -help");
            await Assert.That(first.IsCompleted).IsFalse();
        }
        finally
        {
            executor.ReleaseFirst.TrySetResult();
            await first;
        }
    }

    [Test]
    [Timeout(30_000)]
    public async Task Waiting_For_Stacks_Manifest_Can_Be_Canceled(CancellationToken cancellationToken)
    {
        var executor = new ManifestCacheExecutor();
        var scraper = new TestScraper(executor);
        var first = scraper.ReadHelp(["terraform", "stacks", "list"], cancellationToken);
        using var waitingCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            await executor.FirstStarted.Task.WaitAsync(cancellationToken);
            var waiting = scraper.ReadHelp(["terraform", "stacks", "deployment-run"], waitingCancellation.Token);
            await waitingCancellation.CancelAsync();
            await Assert.That(async () => await waiting).Throws<OperationCanceledException>();
        }
        finally
        {
            executor.ReleaseFirst.TrySetResult();
            await first;
        }

        var later = await scraper.ReadHelp(["terraform", "stacks", "deployment-run"], cancellationToken);
        await Assert.That(later).IsEqualTo("Usage: stacks deployment-run -help");
    }

    private sealed class TestScraper(ICliCommandExecutor executor) : TerraformCliScraper(
        executor,
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<TerraformCliScraper>.Instance)
    {
        public Task<string?> ReadHelp(string[] path, CancellationToken cancellationToken) =>
            GetHelpTextAsync(path, cancellationToken);
    }

    private sealed class ManifestCacheExecutor : ICliCommandExecutor
    {
        private int _activeStacksCalls;
        public TaskCompletionSource FirstStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource ReleaseFirst { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public async Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            if (!arguments.StartsWith("stacks ", StringComparison.Ordinal))
            {
                return Help(arguments);
            }

            var active = Interlocked.Increment(ref _activeStacksCalls);
            try
            {
                // Terraform truncates manifest.json before writing it. Another help process
                // can read the incomplete file and panic while dereferencing the decoded manifest.
                if (active > 1)
                {
                    return new CliCommandResult
                    {
                        ExitCode = 11,
                        StandardOutput = "",
                        StandardError = "panic: runtime error: invalid memory address or nil pointer dereference",
                    };
                }

                if (arguments == "stacks list -help")
                {
                    FirstStarted.TrySetResult();
                    await ReleaseFirst.Task.WaitAsync(cancellationToken);
                }

                return Help(arguments);
            }
            finally
            {
                Interlocked.Decrement(ref _activeStacksCalls);
            }
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        private static CliCommandResult Help(string arguments) => new()
        {
            ExitCode = 0,
            StandardOutput = $"Usage: {arguments}",
            StandardError = "",
        };
    }
}
