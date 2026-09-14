using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ResilientCliCommandExecutorTests
{
    [Test]
    public async Task Real_Negative_Exit_Code_Is_Not_Retried_Or_Marked_Unavailable()
    {
        var response = new CliCommandResult
        {
            ExitCode = -1,
            HasProcessExitCode = true,
            StandardOutput = "Real command output",
            StandardError = "Invalid arguments",
        };
        var inner = new SequenceExecutor(response, response);
        var result = await CreateExecutor(inner, maxRetries: 1).ExecuteAsync("test", "--help");

        await Assert.That(result).IsSameReferenceAs(response);
        await Assert.That(result.Unavailable).IsFalse();
        await Assert.That(inner.ExecutionCount).IsEqualTo(1);
    }

    [Test]
    [Arguments("Temporary process failure", 2)]
    [Arguments("Executable not found", 1)]
    public async Task Final_Legacy_System_Failures_Are_Unavailable(string error, int attempts)
    {
        var failure = new CliCommandResult
        {
            ExitCode = -1,
            StandardOutput = "Launcher diagnostics",
            StandardError = error,
        };
        var inner = new SequenceExecutor(failure, failure);
        var result = await CreateExecutor(inner, maxRetries: 1).ExecuteAsync("test", "--help");

        await Assert.That(result.Unavailable).IsTrue();
        await Assert.That(result.ExecutionFailed).IsTrue();
        await Assert.That(result.ExitCode).IsEqualTo(-1);
        await Assert.That(result.StandardOutput).IsEqualTo(failure.StandardOutput);
        await Assert.That(result.StandardError).IsEqualTo(error);
        await Assert.That(inner.ExecutionCount).IsEqualTo(attempts);
    }

    [Test]
    [Arguments(1, false, true)]
    [Arguments(124, true, false)]
    public async Task Retries_Explicit_System_Failures_Regardless_Of_Exit_Code(
        int exitCode, bool timedOut, bool executionFailed)
    {
        var inner = new SequenceExecutor(new CliCommandResult
        {
            ExitCode = exitCode,
            TimedOut = timedOut,
            ExecutionFailed = executionFailed,
            StandardOutput = string.Empty,
            StandardError = "Temporary process failure",
        }, Success());
        var result = await CreateExecutor(inner, maxRetries: 1).ExecuteAsync("test", string.Empty);

        await Assert.That(result.Success).IsTrue();
        await Assert.That(inner.ExecutionCount).IsEqualTo(2);
    }

    [Test]
    [Arguments(1, true, false, "Executable not found")]
    [Arguments(1, false, false, "Invalid arguments")]
    [Arguments(-2, false, true, "Circuit open")]
    public async Task Does_Not_Retry_Permanent_Or_Tool_Failures(
        int exitCode, bool executionFailed, bool circuitOpen, string error)
    {
        var failure = new CliCommandResult
        {
            ExitCode = exitCode,
            ExecutionFailed = executionFailed,
            CircuitOpen = circuitOpen,
            StandardOutput = string.Empty,
            StandardError = error,
        };
        var inner = new SequenceExecutor(failure, Success());
        var result = await CreateExecutor(inner, maxRetries: 1).ExecuteAsync("test", string.Empty);

        await Assert.That(result).IsSameReferenceAs(failure);
        await Assert.That(inner.ExecutionCount).IsEqualTo(1);
    }

    [Test]
    public async Task CircuitBreaker_Counts_Explicit_Launch_Failures()
    {
        var failure = new CliCommandResult
        {
            ExitCode = 1,
            ExecutionFailed = true,
            StandardOutput = string.Empty,
            StandardError = "Temporary process failure",
        };
        var inner = new SequenceExecutor([.. Enumerable.Repeat(failure, 5), Success()]);
        var results = await ExecuteAsync(CreateExecutor(inner), 6);

        await Assert.That(results[5].CircuitOpen).IsTrue();
        await Assert.That(inner.ExecutionCount).IsEqualTo(5);
    }

    [Test]
    public async Task ToolSpecificAvailabilityProbe_BypassesResilienceShield()
    {
        var inner = new RecordingExecutor();
        var executor = new ResilientCliCommandExecutor(
            inner,
            NullLogger<ResilientCliCommandExecutor>.Instance);

        var isAvailable = await executor.IsAvailableAsync("kubectl", "version --client");

        await Assert.That(isAvailable).IsTrue();
        await Assert.That(inner.AvailabilityProbes).IsEquivalentTo([("kubectl", "version --client")]);
        await Assert.That(inner.ExecutionCount).IsEqualTo(0);
    }

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
        var inner = new SequenceExecutor([.. Enumerable.Repeat(Failure(), 20)]);
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

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<(string Command, string Arguments)> AvailabilityProbes { get; } = [];

        public int ExecutionCount { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            ExecutionCount++;
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = "transient failure",
                ExitCode = -1
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
            AvailabilityProbes.Add((command, arguments));
            return Task.FromResult(true);
        }
    }
}
