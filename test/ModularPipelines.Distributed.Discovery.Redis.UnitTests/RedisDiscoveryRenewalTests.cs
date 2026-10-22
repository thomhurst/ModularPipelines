using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisDiscoveryRenewalTests
{
    private static readonly MasterEndpoint Endpoint = new() { Url = new Uri("https://master.example/"), AccessToken = "secret-token" };
    private static readonly DistributedRunCleanupContext CleanupContext = new() { RunId = "test-run", PipelineSucceeded = true };

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Advertisement_Retries_A_Failed_Write_And_Renews_The_New_Endpoint(bool writeBeforeFailure)
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock) { FailNextWrite = true, WriteBeforeFailure = writeBeforeFailure };
        await using var master = Create(store, clock);
        await Assert.That(async () => await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None))
            .Throws<TimeoutException>();

        clock.Advance(TimeSpan.FromSeconds(1));
        await Assert.That(store.RefreshCount).IsEqualTo(0);
        var retryEndpoint = new MasterEndpoint { Url = new Uri("https://retry.example/"), AccessToken = "new-token" };
        await master.AdvertiseMasterEndpointAsync(retryEndpoint, CancellationToken.None);

        for (var i = 0; i < 6; i++)
        {
            clock.Advance(TimeSpan.FromSeconds(1));
            await store.NextRefreshAsync();
        }

        await Assert.That(await master.DiscoverMasterEndpointAsync(CancellationToken.None)).IsEqualTo(retryEndpoint);
        await master.CleanupRunAsync(CleanupContext, CancellationToken.None);
        await Assert.That(store.Value).IsNull();
    }

    [Test]
    public async Task Failed_Retry_Does_Not_Orphan_An_Ambiguous_Advertisement()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock) { FailNextWrite = true, WriteBeforeFailure = true };
        await using var master = Create(store, clock);
        await Assert.That(async () => await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None))
            .Throws<TimeoutException>();
        await Assert.That(store.Value).IsNotNull();

        store.FailNextWrite = true;
        store.WriteBeforeFailure = false;
        await Assert.That(async () => await master.AdvertiseMasterEndpointAsync(
                new MasterEndpoint { Url = new Uri("https://retry.example/"), AccessToken = "new-token" }, CancellationToken.None))
            .Throws<TimeoutException>();
        await master.CleanupRunAsync(CleanupContext, CancellationToken.None);

        await Assert.That(store.Value).IsNull();
    }

    [Test]
    public async Task Successful_Advertisement_Cannot_Be_Replaced_By_The_Same_Instance()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        await using var master = Create(store, clock);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);

        await Assert.That(async () => await master.AdvertiseMasterEndpointAsync(
                new MasterEndpoint { Url = new Uri("https://retry.example/") }, CancellationToken.None))
            .Throws<InvalidOperationException>();
        await Assert.That(await master.DiscoverMasterEndpointAsync(CancellationToken.None)).IsEqualTo(Endpoint);
    }

    [Test]
    public async Task Late_Worker_Discovers_Master_After_The_Original_Ttl()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        await using var master = Create(store, clock);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);

        for (var i = 0; i < 6; i++)
        {
            clock.Advance(TimeSpan.FromSeconds(1));
            await store.NextRefreshAsync();
        }

        await using var worker = Create(store, clock);
        await Assert.That(await worker.DiscoverMasterEndpointAsync(CancellationToken.None)).IsEqualTo(Endpoint);
    }

    [Test]
    public async Task Cleanup_Stops_Renewal_And_Does_Not_Recreate_The_Endpoint()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        await using var master = Create(store, clock);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        await master.CleanupRunAsync(CleanupContext, CancellationToken.None);

        clock.Advance(TimeSpan.FromHours(1));

        await Assert.That(store.Value).IsNull();
        await Assert.That(store.RefreshCount).IsEqualTo(0);
        await Assert.That(async () => await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None))
            .Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task Disposal_Stops_Renewal_When_Run_Cleanup_Is_Skipped()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        await using var master = Create(store, clock);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        await master.DisposeAsync();
        await Assert.That(store.Value).IsNotNull();

        clock.Advance(TimeSpan.FromSeconds(4));

        await Assert.That(store.Value).IsNull();
        await Assert.That(store.RefreshCount).IsEqualTo(0);
    }

    [Test]
    public async Task Old_Master_Cannot_Renew_Or_Delete_A_Replacement_With_The_Same_Endpoint()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        await using var oldMaster = Create(store, clock);
        await using var replacement = Create(store, clock);
        await oldMaster.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        var oldValue = store.Value;
        await replacement.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        var replacementValue = store.Value;
        await replacement.DisposeAsync();

        clock.Advance(TimeSpan.FromSeconds(1));
        await store.NextRefreshAsync();
        await oldMaster.CleanupRunAsync(CleanupContext, CancellationToken.None);

        await Assert.That(replacementValue).IsNotEqualTo(oldValue);
        await Assert.That(store.Value).IsEqualTo(replacementValue);
        clock.Advance(TimeSpan.FromSeconds(3));
        await Assert.That(store.Value).IsNull();
    }

    [Test]
    public async Task Missing_Advertisement_Is_Not_Recreated_And_Logs_A_Warning()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock);
        var logger = new RecordingLogger();
        await using var master = Create(store, clock, logger);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        store.Remove();

        clock.Advance(TimeSpan.FromSeconds(1));
        await store.NextRefreshAsync();
        await master.DisposeAsync();
        clock.Advance(TimeSpan.FromHours(1));

        await Assert.That(store.Value).IsNull();
        await Assert.That(store.RefreshCount).IsEqualTo(1);
        await Assert.That(logger.Messages.Any(m => m.Level == LogLevel.Warning)).IsTrue();
    }

    [Test]
    public async Task Renewal_Retries_A_Failure_Without_Logging_The_Token()
    {
        var clock = new FakeTimeProvider();
        var store = new ExpiringStore(clock) { FailNextRefresh = true };
        var logger = new RecordingLogger();
        await using var master = Create(store, clock, logger);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);

        for (var i = 0; i < 4; i++)
        {
            clock.Advance(TimeSpan.FromSeconds(1));
            await store.NextRefreshAsync();
        }
        await master.DisposeAsync();

        await Assert.That(store.Value).IsNotNull();
        await Assert.That(logger.Messages.Any(m => m.Level == LogLevel.Warning)).IsTrue();
        await Assert.That(logger.Messages.Any(m => m.Message.Contains("secret-token"))).IsFalse();
    }

    [Test]
    public async Task Cleanup_Waits_For_An_InFlight_Renewal_Before_Deleting()
    {
        var clock = new FakeTimeProvider();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var store = new ExpiringStore(clock) { RefreshGate = release.Task };
        await using var master = Create(store, clock);
        await master.AdvertiseMasterEndpointAsync(Endpoint, CancellationToken.None);
        clock.Advance(TimeSpan.FromSeconds(1));
        await store.RefreshEntered.Task.WaitAsync(TimeSpan.FromSeconds(5));

        var cleanup = master.CleanupRunAsync(CleanupContext, CancellationToken.None);
        try
        {
            await Assert.That(cleanup.IsCompleted).IsFalse();
            await Assert.That(store.Value).IsNotNull();
        }
        finally
        {
            release.TrySetResult();
        }
        await cleanup.WaitAsync(TimeSpan.FromSeconds(5));
        clock.Advance(TimeSpan.FromHours(1));
        await Assert.That(store.Value).IsNull();
    }

    private static RedisMasterDiscovery Create(ExpiringStore store, FakeTimeProvider clock, ILogger<RedisMasterDiscovery>? logger = null) =>
        new(store, new RedisDiscoveryOptions { TimeToLive = TimeSpan.FromSeconds(3) },
            new DistributedOptions { RunId = "test-run" }, logger ?? NullLogger<RedisMasterDiscovery>.Instance, clock);

    private sealed class ExpiringStore(FakeTimeProvider clock) : IRedisDiscoveryStore
    {
        private readonly Channel<bool> _refreshes = Channel.CreateUnbounded<bool>();
        private string? _value;
        private DateTimeOffset _expiresAt;
        public int RefreshCount { get; private set; }
        public bool FailNextWrite { get; set; }
        public bool WriteBeforeFailure { get; set; }
        public bool FailNextRefresh { get; set; }
        public Task? RefreshGate { get; init; }
        public TaskCompletionSource RefreshEntered { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public string? Value => clock.GetUtcNow() < _expiresAt ? _value : null;

        public void Remove() => _value = null;

        public Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken cancellationToken)
        {
            var fail = FailNextWrite;
            FailNextWrite = false;
            if (fail && !WriteBeforeFailure)
            {
                return Task.FromException(new TimeoutException("Simulated failure before the write"));
            }
            _value = value;
            _expiresAt = clock.GetUtcNow() + ttl;
            return fail ? Task.FromException(new TimeoutException("Simulated failure after the write")) : Task.CompletedTask;
        }

        public Task<string?> GetAsync(string key, CancellationToken cancellationToken) => Task.FromResult(Value);

        public async Task<bool> RefreshAsync(string key, string expectedValue, TimeSpan ttl, CancellationToken cancellationToken)
        {
            RefreshCount++;
            RefreshEntered.TrySetResult();
            if (RefreshGate is { } gate)
            {
                // Deliberately ignore cancellation to exercise cleanup waiting for an already sent command.
                await gate;
            }

            try
            {
                if (FailNextRefresh)
                {
                    FailNextRefresh = false;
                    throw new InvalidOperationException("command failed: secret-token");
                }
                if (Value != expectedValue)
                {
                    return false;
                }
                _expiresAt = clock.GetUtcNow() + ttl;
                return true;
            }
            finally
            {
                _refreshes.Writer.TryWrite(true);
            }
        }

        public async Task NextRefreshAsync() => await _refreshes.Reader.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

        public Task DeleteAsync(string key, string expectedValue, CancellationToken cancellationToken)
        {
            if (Value == expectedValue)
            {
                _value = null;
            }
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingLogger : ILogger<RedisMasterDiscovery>
    {
        public System.Collections.Concurrent.ConcurrentQueue<(LogLevel Level, string Message)> Messages { get; } = new();
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            Messages.Enqueue((logLevel, formatter(state, exception) + exception));
    }
}
