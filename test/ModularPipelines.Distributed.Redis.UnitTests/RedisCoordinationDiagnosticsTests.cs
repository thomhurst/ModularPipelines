using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class RedisCoordinationDiagnosticsTests
{
    [Test]
    public async Task Successful_Probe_Logs_Only_Safe_Server_Metadata_And_Disposes_Its_Client()
    {
        var (client, server) = CreateClient();
        server.Setup(commands => commands.SlowLogAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(
            [new RespireSlowLogEntry(1, DateTimeOffset.UnixEpoch, TimeSpan.FromMilliseconds(250),
                ["AUTH", "secret-password"], "private-address", "private-name")]);
        server.Setup(commands => commands.InfoAsync("stats", It.IsAny<CancellationToken>()))
            .ReturnsAsync("latest_fork_usec:123\nrejected_connections:2\nevicted_keys:0\ntotal_error_replies:invalid\nprivate-field:secret-value");
        var logger = new CapturingLogger();
        await using var diagnostics = new RedisCoordinationDiagnostics(_ => Task.FromResult(client.Object), logger);

        await diagnostics.ReportFailure("lease heartbeat");

        var messages = string.Join('\n', logger.Messages);
        await Assert.That(messages).Contains("PING succeeded").And.Contains("250 ms")
            .And.Contains("latest_fork_usec: 123").And.Contains("rejected_connections: 2");
        await Assert.That(messages).DoesNotContain("secret").And.DoesNotContain("private").And.DoesNotContain("invalid");
        client.Verify(connection => connection.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task Permission_Denial_Does_Not_Hide_Ping_Or_Other_Diagnostics()
    {
        var (client, server) = CreateClient();
        server.Setup(commands => commands.SlowLogAsync(5, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RespireServerException("NOPERM private-endpoint secret"));
        server.Setup(commands => commands.InfoAsync("stats", It.IsAny<CancellationToken>()))
            .ReturnsAsync("total_error_replies:3");
        var logger = new CapturingLogger();
        await using var diagnostics = new RedisCoordinationDiagnostics(_ => Task.FromResult(client.Object), logger);

        await diagnostics.ReportFailure("master liveness");

        var messages = string.Join('\n', logger.Messages);
        await Assert.That(messages).Contains("PING succeeded").And.Contains("SLOWLOG diagnostics unavailable")
            .And.Contains("total_error_replies: 3").And.DoesNotContain("secret");
    }

    [Test]
    public async Task Unreachable_Probe_Is_Bounded_And_Reports_Unknown_Reachability()
    {
        var logger = new CapturingLogger();
        await using var diagnostics = new RedisCoordinationDiagnostics(async token =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            throw new InvalidOperationException("unreachable");
        }, logger);

        await diagnostics.ReportFailure("lease heartbeat").WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(string.Join('\n', logger.Messages)).Contains("reachability is unknown")
            .And.Contains("3000 ms").And.Contains("Lease deadlines remain enforced");
    }

    [Test]
    public async Task Concurrent_Failures_Coalesce_And_Completed_Probes_Are_Rate_Limited()
    {
        var (client, _) = CreateClient();
        var connection = new TaskCompletionSource<IRespireClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var attempts = 0;
        var clock = new ManualClock();
        await using var diagnostics = new RedisCoordinationDiagnostics(_ =>
        {
            Interlocked.Increment(ref attempts);
            return connection.Task;
        }, new CapturingLogger(), clock);

        var first = diagnostics.ReportFailure("lease heartbeat");
        clock.Advance(RedisCoordinationDiagnostics.ProbeInterval);
        var concurrent = diagnostics.ReportFailure("master liveness");
        await Assert.That(concurrent).IsSameReferenceAs(first);
        connection.SetResult(client.Object);
        await first;
        await Assert.That(attempts).IsEqualTo(1);

        await diagnostics.ReportFailure("master liveness");
        await Assert.That(attempts).IsEqualTo(2);
        await diagnostics.ReportFailure("lease heartbeat");
        await Assert.That(attempts).IsEqualTo(2);
        clock.Advance(RedisCoordinationDiagnostics.ProbeInterval);
        await diagnostics.ReportFailure("lease heartbeat");
        await Assert.That(attempts).IsEqualTo(3);
    }

    [Test]
    public async Task Disposal_Waits_For_Active_Probe_And_Its_Client_Disposal()
    {
        var (client, _) = CreateClient();
        var probeStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var connection = new TaskCompletionSource<IRespireClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var clientDisposalStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var allowClientDisposal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        client.Setup(value => value.DisposeAsync()).Returns(() =>
        {
            clientDisposalStarted.TrySetResult();
            return new ValueTask(allowClientDisposal.Task);
        });
        var diagnostics = new RedisCoordinationDiagnostics(_ =>
        {
            probeStarted.TrySetResult();
            return connection.Task;
        }, new CapturingLogger());
        var probe = diagnostics.ReportFailure("heartbeat");
        try
        {
            await probeStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var disposal = diagnostics.DisposeAsync().AsTask();

            await Assert.That(disposal.IsCompleted).IsFalse();
            client.Verify(value => value.DisposeAsync(), Times.Never);
            connection.SetResult(client.Object);
            await clientDisposalStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));

            await Assert.That(disposal.IsCompleted).IsFalse();
            allowClientDisposal.SetResult();
            await disposal.WaitAsync(TimeSpan.FromSeconds(10));
            await Assert.That(probe.IsCompletedSuccessfully).IsTrue();
            client.Verify(value => value.DisposeAsync(), Times.Once);
        }
        finally
        {
            connection.TrySetResult(client.Object);
            allowClientDisposal.TrySetResult();
            await diagnostics.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(10));
        }
    }

    [Test]
    public async Task Disposal_And_Disabled_Logging_Prevent_New_Probes()
    {
        var attempts = 0;
        Task<IRespireClient> Connect(CancellationToken _) => throw new InvalidOperationException($"attempt {++attempts}");
        await using var disabled = new RedisCoordinationDiagnostics(Connect, null);
        await disabled.ReportFailure("heartbeat");
        var disposed = new RedisCoordinationDiagnostics(Connect, new CapturingLogger());
        await disposed.DisposeAsync();
        await disposed.ReportFailure("heartbeat");

        await Assert.That(attempts).IsEqualTo(0);
    }

    [Test]
    [Arguments("master liveness")]
    [Arguments("lease heartbeat")]
    [Arguments("worker heartbeat")]
    [Arguments("master heartbeat")]
    public async Task Coordinator_Rethrows_Original_Timeout_Without_Waiting_For_Probe(string operation)
    {
        var failure = new RespireTimeoutException("EVALSHA", TimeSpan.FromSeconds(15));
        var scripts = new Mock<IRespireScriptCommands>();
        scripts.Setup(commands => commands.ExecuteIntegerAsync(
                It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        scripts.Setup(commands => commands.ExecuteAsync(
                It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var client = new Mock<IRespireClient>();
        client.SetupGet(connection => connection.Scripts).Returns(scripts.Object);
        var pending = new TaskCompletionSource<IRespireClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var logger = new CapturingLogger();
        await using var diagnostics = new RedisCoordinationDiagnostics(_ => pending.Task, logger);
        await using var coordinator = new RedisDistributedCoordinator(client.Object,
            new RedisKeyBuilder("test", "run"), new RedisOptions(), diagnostics: diagnostics);
        try
        {
            Exception? observed = null;
            try
            {
                var call = operation switch
                {
                    "master liveness" => coordinator.GetMasterStateAsync(CancellationToken.None),
                    "lease heartbeat" => coordinator.ReconcileHeartbeatAsync(new WorkerStatus { WorkerId = new WorkerId("worker") }, [], CancellationToken.None),
                    "worker heartbeat" => coordinator.SendHeartbeatAsync(new WorkerStatus { WorkerId = new WorkerId("worker") }, CancellationToken.None),
                    _ => coordinator.SendMasterHeartbeatAsync(CancellationToken.None),
                };
                await call.WaitAsync(TimeSpan.FromSeconds(1));
            }
            catch (Exception exception)
            {
                observed = exception;
            }

            await Assert.That(observed).IsSameReferenceAs(failure);
            await Assert.That(pending.Task.IsCompleted).IsFalse();
        }
        finally
        {
            pending.TrySetException(new RespireConnectionException("private-endpoint secret"));
        }

        await diagnostics.DisposeAsync();
        await Assert.That(string.Join('\n', logger.Messages)).Contains(operation).And.DoesNotContain("secret");
    }

    private static (Mock<IRespireClient> Client, Mock<IRespireServerCommands> Server) CreateClient()
    {
        var server = new Mock<IRespireServerCommands>();
        server.Setup(commands => commands.SlowLogAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync([]);
        server.Setup(commands => commands.InfoAsync("stats", It.IsAny<CancellationToken>())).ReturnsAsync("");
        var client = new Mock<IRespireClient>();
        client.SetupGet(connection => connection.Server).Returns(server.Object);
        return (client, server);
    }

    private sealed class ManualClock : TimeProvider
    {
        private long _timestamp;
        public override long TimestampFrequency => TimeSpan.TicksPerSecond;
        public override long GetTimestamp() => _timestamp;
        public void Advance(TimeSpan elapsed) => _timestamp += elapsed.Ticks;
    }

    private sealed class CapturingLogger : ILogger
    {
        public List<string> Messages { get; } = [];
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => Messages.Add(formatter(state, exception));
    }
}
