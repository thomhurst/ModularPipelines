using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ResilientCliCommandExecutorTests
{
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
