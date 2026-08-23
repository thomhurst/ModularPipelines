using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ResilientCliCommandExecutorTests
{
    [Test]
    public async Task CircuitBreaker_StaysClosed_WhenFailuresAreInterleavedWithSuccesses()
    {
        var inner = new SequenceExecutor(
            Failure(), Success(),
            Failure(), Success(),
            Failure(), Success(),
            Failure(), Success(),
            Failure(), Success());
        var executor = CreateExecutor(inner);

        var results = await ExecuteAsync(executor, 10);

        await Assert.That(results.Select(result => result.ExitCode)).DoesNotContain(-2);
        await Assert.That(inner.ExecutionCount).IsEqualTo(10);
    }

    [Test]
    public async Task CircuitBreaker_Opens_AfterFiveConsecutiveFailures()
    {
        var inner = new SequenceExecutor(
            Failure(), Failure(), Failure(), Failure(), Failure(), Success());
        var executor = CreateExecutor(inner);

        var results = await ExecuteAsync(executor, 6);

        await Assert.That(results.Take(5).Select(result => result.ExitCode))
            .IsEquivalentTo(Enumerable.Repeat(-1, 5));
        await Assert.That(results[5].ExitCode).IsEqualTo(-2);
        await Assert.That(inner.ExecutionCount).IsEqualTo(5);
    }

    [Test]
    public async Task CircuitBreaker_CountsFailedCommands_NotIndividualRetryAttempts()
    {
        var inner = new SequenceExecutor(Enumerable.Repeat(Failure(), 20).ToArray());
        var executor = CreateExecutor(inner, maxRetries: 3);

        var results = await ExecuteAsync(executor, 6);

        await Assert.That(results.Take(5).Select(result => result.ExitCode))
            .IsEquivalentTo(Enumerable.Repeat(-1, 5));
        await Assert.That(results[5].ExitCode).IsEqualTo(-2);
        await Assert.That(inner.ExecutionCount).IsEqualTo(20);
    }

    private static ResilientCliCommandExecutor CreateExecutor(ICliCommandExecutor inner, int maxRetries = 0)
        => new(
            inner,
            NullLogger<ResilientCliCommandExecutor>.Instance,
            maxRetries,
            baseDelay: TimeSpan.Zero,
            circuitBreakerThreshold: 5,
            circuitBreakerDuration: TimeSpan.FromMinutes(1));

    private static async Task<IReadOnlyList<CliCommandResult>> ExecuteAsync(
        ResilientCliCommandExecutor executor,
        int count)
    {
        var results = new List<CliCommandResult>(count);
        for (var i = 0; i < count; i++)
        {
            results.Add(await executor.ExecuteAsync("test", string.Empty));
        }

        return results;
    }

    private static CliCommandResult Failure() => new()
    {
        ExitCode = -1,
        StandardOutput = string.Empty,
        StandardError = "timeout",
    };

    private static CliCommandResult Success() => new()
    {
        ExitCode = 0,
        StandardOutput = string.Empty,
        StandardError = string.Empty,
    };

    private sealed class SequenceExecutor(params CliCommandResult[] results) : ICliCommandExecutor
    {
        private readonly Queue<CliCommandResult> _results = new(results);

        public int ExecutionCount { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            ExecutionCount++;
            return Task.FromResult(_results.Dequeue());
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default)
            => Task.FromResult(true);
    }
}
