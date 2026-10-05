using System.Collections.Concurrent;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class AzCliMetadataExecutorTests
{
    [Test]
    public async Task ConcurrentHelpUsesOnePythonDiscoveryAndPreservesArguments()
    {
        var inner = new Executor();
        var executor = new AzCliMetadataExecutor(inner);
        var results = await Task.WhenAll(Enumerable.Range(0, 4)
            .Select(_ => executor.ExecuteAsync("az", "acr login --help", workingDirectory: "work")));

        await Assert.That(inner.Calls.Count(call => call.Arguments == "--version")).IsEqualTo(1);
        await Assert.That(inner.Calls.Where(call => call.Command == "azure-python")
            .All(call => call.Arguments.EndsWith(" acr login --help", StringComparison.Ordinal) && call.Directory == "work")).IsTrue();
        await Assert.That(results.All(result => result.StandardOutput.Contains(AzCliMetadataExecutor.MetadataMarker))).IsTrue();
    }

    [Test]
    public async Task MissingPythonLocationStopsUnverifiedGeneration()
    {
        var executor = new AzCliMetadataExecutor(new Executor { Version = "azure-cli 2.84.0" });
        async Task Act() => _ = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(Act)
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task MissingArgumentMetadataStopsUnverifiedGeneration()
    {
        var executor = new AzCliMetadataExecutor(new Executor { Help = "Command\n    az acr login : Log in." });
        async Task Act() => _ = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(Act).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task NonHelpCommandsPassThrough()
    {
        var inner = new Executor();
        var executor = new AzCliMetadataExecutor(inner);
        await executor.ExecuteAsync("custom-az", "version");
        await Assert.That(inner.Calls.Single().Command).IsEqualTo("custom-az");
        await Assert.That(inner.Calls.Single().Arguments).IsEqualTo("version");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task FailedDiscoveryCanRecover(bool cancelled)
    {
        var attempts = 0;
        var inner = new Executor
        {
            Discover = _ => ++attempts == 1
                ? cancelled
                    ? Task.FromCanceled<CliCommandResult>(new CancellationToken(true))
                    : Task.FromException<CliCommandResult>(new IOException("temporary discovery failure"))
                : Task.FromResult(VersionResult()),
        };
        var executor = new AzCliMetadataExecutor(inner);
        async Task FirstAttempt() => _ = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(FirstAttempt).ThrowsException();

        var result = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(result.Success).IsTrue();
        await Assert.That(attempts).IsEqualTo(2);
    }

    [Test]
    public async Task CancelledWaiterDoesNotEvictPendingSharedDiscovery()
    {
        var discovery = new TaskCompletionSource<CliCommandResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var inner = new Executor { Discover = _ => discovery.Task };
        var executor = new AzCliMetadataExecutor(inner);
        var first = executor.ExecuteAsync("az", "acr login --help");
        using var cancellation = new CancellationTokenSource();
        var waiter = executor.ExecuteAsync("az", "account show --help", cancellation.Token);
        cancellation.Cancel();
        async Task WaitForCancelledCaller() => _ = await waiter;
        await Assert.That(WaitForCancelledCaller).Throws<OperationCanceledException>();
        discovery.SetResult(VersionResult());
        await first;
        await executor.ExecuteAsync("az", "account show --help");
        await Assert.That(inner.Calls.Count(call => call.Arguments == "--version")).IsEqualTo(1);
    }

    [Test]
    public async Task DiscoveryFailureAfterCallerCancellationIsEvicted()
    {
        var discovery = new TaskCompletionSource<CliCommandResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var attempts = 0;
        var inner = new Executor
        {
            Discover = _ => ++attempts == 1 ? discovery.Task : Task.FromResult(VersionResult()),
        };
        var executor = new AzCliMetadataExecutor(inner);
        using var cancellation = new CancellationTokenSource();
        var first = executor.ExecuteAsync("az", "acr login --help", cancellation.Token);
        var observer = executor.ExecuteAsync("az", "account show --help");
        cancellation.Cancel();
        async Task WaitForFirst() => _ = await first;
        await Assert.That(WaitForFirst).Throws<OperationCanceledException>();

        discovery.SetException(new IOException("late discovery failure"));
        async Task WaitForObserver() => _ = await observer;
        await Assert.That(WaitForObserver).Throws<IOException>();
        await executor.ExecuteAsync("az", "account show --help");
        await Assert.That(attempts).IsEqualTo(2);
    }

    private static CliCommandResult VersionResult() => new()
    {
        ExitCode = 0,
        StandardError = string.Empty,
        StandardOutput = "azure-cli 2.84.0\nPython location 'azure-python'",
    };

    private sealed class Executor : ICliCommandExecutor
    {
        public Func<CancellationToken, Task<CliCommandResult>>? Discover { get; init; }

        public string Version { get; init; } = "azure-cli 2.84.0\nPython location 'azure-python'";

        public string Help { get; init; } = AzCliMetadataExecutor.MetadataMarker + "{}";

        public ConcurrentQueue<(string Command, string Arguments, string? Directory)> Calls { get; } = new();

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            Calls.Enqueue((command, arguments, workingDirectory));
            if (arguments == "--version" && Discover is not null)
            {
                return Discover(cancellationToken);
            }

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments == "--version" ? Version : Help,
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
