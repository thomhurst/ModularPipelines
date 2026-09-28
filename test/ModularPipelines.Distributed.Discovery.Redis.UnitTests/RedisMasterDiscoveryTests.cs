using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisMasterDiscoveryTests
{
    [Test]
    public async Task Advertised_Endpoint_RoundTrips_Url_And_Token()
    {
        var store = new FakeStore();
        var discovery = Create(store);
        var endpoint = new MasterEndpoint { Url = new Uri("https://master.example/"), AccessToken = "secret" };

        await discovery.AdvertiseMasterEndpointAsync(endpoint, CancellationToken.None);
        var discovered = await discovery.DiscoverMasterEndpointAsync(CancellationToken.None);

        await Assert.That(discovered).IsEqualTo(endpoint);
        await Assert.That(store.Ttls.Single()).IsEqualTo(TimeSpan.FromMinutes(10));
        await Assert.That(store.Values.Keys.Single()).IsEqualTo("test-prefix:test-run:master-endpoint");
    }

    [Test]
    public async Task Discovery_Never_Logs_The_Access_Token()
    {
        var store = new FakeStore();
        var logger = new RecordingLogger();
        var discovery = new RedisMasterDiscovery(
            store,
            new RedisDiscoveryOptions { KeyPrefix = "test-prefix" },
            new DistributedOptions { RunId = "test-run" },
            logger);

        await discovery.AdvertiseMasterEndpointAsync(
            new MasterEndpoint { Url = new Uri("https://tunnel.example/"), AccessToken = "secret-token" },
            CancellationToken.None);
        await discovery.DiscoverMasterEndpointAsync(CancellationToken.None);

        await Assert.That(logger.Messages.Any(message => message.Message.Contains("secret-token"))).IsFalse();
        await Assert.That(logger.Messages
                .Where(message => message.Level >= LogLevel.Information)
                .Any(message => message.Message.Contains("tunnel.example")))
            .IsFalse();
    }

    [Test]
    public async Task Discovery_Polls_Until_The_Endpoint_Is_Available()
    {
        var store = new FakeStore { MissesBeforeHit = 2 };
        var discovery = Create(store, pollInterval: TimeSpan.FromMilliseconds(20));
        await discovery.AdvertiseMasterEndpointAsync(
            new MasterEndpoint { Url = new Uri("http://master:5099") },
            CancellationToken.None);

        var endpoint = await discovery.DiscoverMasterEndpointAsync(CancellationToken.None);

        await Assert.That(endpoint.Url).IsEqualTo(new Uri("http://master:5099"));
        await Assert.That(store.Reads).IsGreaterThanOrEqualTo(3);
    }

    [Test]
    public async Task Discovery_Times_Out()
    {
        var discovery = Create(new FakeStore(), pollInterval: TimeSpan.FromMilliseconds(50), timeout: TimeSpan.FromMilliseconds(300));

        await Assert.That(async () => await discovery.DiscoverMasterEndpointAsync(CancellationToken.None))
            .Throws<TimeoutException>();
    }

    [Test]
    public async Task Discovery_Propagates_Caller_Cancellation()
    {
        var discovery = Create(new FakeStore(), pollInterval: TimeSpan.FromMilliseconds(50), timeout: TimeSpan.FromSeconds(30));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await Assert.That(async () => await discovery.DiscoverMasterEndpointAsync(cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task Discovery_Does_Not_Translate_A_Store_Cancellation_Into_A_Timeout()
    {
        var discovery = Create(new FakeStore { ThrowOnRead = new OperationCanceledException("store cancelled") });

        await Assert.That(async () => await discovery.DiscoverMasterEndpointAsync(CancellationToken.None))
            .ThrowsExactly<OperationCanceledException>();
    }

    [Test]
    public async Task Invalid_Stored_Endpoint_Is_Reported()
    {
        var store = new FakeStore();
        store.Values["test-prefix:test-run:master-endpoint"] = "http://legacy-plain-url";

        await Assert.That(async () => await Create(store).DiscoverMasterEndpointAsync(CancellationToken.None))
            .Throws<InvalidOperationException>();
    }

    private static RedisMasterDiscovery Create(
        FakeStore store,
        TimeSpan? pollInterval = null,
        TimeSpan? timeout = null) =>
        new(
            store,
            new RedisDiscoveryOptions
            {
                KeyPrefix = "test-prefix",
                TimeToLive = TimeSpan.FromMinutes(10),
                PollInterval = pollInterval ?? TimeSpan.FromMilliseconds(50),
                DiscoveryTimeout = timeout ?? TimeSpan.FromSeconds(5),
            },
            new DistributedOptions { RunId = "test-run" },
            NullLogger<RedisMasterDiscovery>.Instance);

    private sealed class FakeStore : IRedisDiscoveryStore
    {
        private int _reads;

        public ConcurrentDictionary<string, string> Values { get; } = new();

        public List<TimeSpan> Ttls { get; } = [];

        public int MissesBeforeHit { get; init; }

        public Exception? ThrowOnRead { get; init; }

        public int Reads => Volatile.Read(ref _reads);

        public Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken cancellationToken)
        {
            Values[key] = value;
            Ttls.Add(ttl);
            return Task.CompletedTask;
        }

        public Task<string?> GetAsync(string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (ThrowOnRead is not null)
            {
                return Task.FromException<string?>(ThrowOnRead);
            }

            var read = Interlocked.Increment(ref _reads);
            return Task.FromResult(read > MissesBeforeHit && Values.TryGetValue(key, out var value) ? value : null);
        }
    }

    private sealed class RecordingLogger : ILogger<RedisMasterDiscovery>
    {
        public ConcurrentQueue<(LogLevel Level, string Message)> Messages { get; } = new();

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Messages.Enqueue((logLevel, formatter(state, exception)));
    }
}
