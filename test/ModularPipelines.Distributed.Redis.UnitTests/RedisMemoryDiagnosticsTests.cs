using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class RedisMemoryDiagnosticsTests
{
    [Test]
    [Arguments("0", "volatile-lru")]
    [Arguments("0", "")]
    [Arguments("1024", "noeviction")]
    [Arguments("1024", "NOEVICTION")]
    public async Task Safe_Configurations_Do_Not_Warn_Or_Fail(string maxMemory, string policy)
    {
        var (client, _) = CreateClient($"# Memory\r\nmaxmemory:{maxMemory}\r\nmaxmemory_policy:{policy}\r\n");
        var logger = new CapturingLogger();

        await RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object,
            new RedisOptions { FailOnUnsafeMemoryPolicy = true }, logger, CancellationToken.None);

        await Assert.That(logger.Messages).IsEmpty();
    }

    [Test]
    [Arguments("volatile-lru")]
    [Arguments("allkeys-lru")]
    [Arguments("volatile-lfu")]
    [Arguments("allkeys-lfu")]
    [Arguments("volatile-random")]
    [Arguments("allkeys-random")]
    [Arguments("volatile-ttl")]
    public async Task Finite_Memory_With_Eviction_Warns(string policy)
    {
        var (client, _) = CreateClient($"maxmemory:1024\nmaxmemory_policy:{policy}\n");
        var logger = new CapturingLogger();

        await RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object, new RedisOptions(), logger, CancellationToken.None);

        await Assert.That(logger.Messages).HasSingleItem();
        await Assert.That(logger.Messages[0]).Contains(policy).And.Contains("1024").And.Contains("evicted");
    }

    [Test]
    [Arguments("")]
    [Arguments("maxmemory:unknown\nmaxmemory_policy:noeviction")]
    [Arguments("maxmemory:-1\nmaxmemory_policy:noeviction")]
    [Arguments("maxmemory:1024")]
    public async Task Missing_Or_Invalid_Configuration_Warns_That_Safety_Is_Unknown(string info)
    {
        var (client, _) = CreateClient(info);
        var logger = new CapturingLogger();

        await RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object, new RedisOptions(), logger, CancellationToken.None);

        await Assert.That(logger.Messages).HasSingleItem();
        await Assert.That(logger.Messages[0]).Contains("eviction safety is unknown");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Denied_Or_Unavailable_Info_Warns_Or_Fails_According_To_Options(bool failFast)
    {
        var (client, server) = CreateClient("");
        server.Setup(commands => commands.InfoAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RespireServerException("NOPERM this user has no permissions to run the 'info' command"));
        var logger = new CapturingLogger();
        Task check() => RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object,
            new RedisOptions { FailOnUnsafeMemoryPolicy = failFast }, logger, CancellationToken.None);

        if (failFast)
        {
            await Assert.That(check).Throws<InvalidOperationException>().WithMessageContaining("eviction safety is unknown");
        }
        else
        {
            await check();
            await Assert.That(logger.Messages).HasSingleItem();
            await Assert.That(logger.Messages[0]).Contains("eviction safety is unknown");
        }
    }

    [Test]
    public async Task Info_Timeout_Is_A_Diagnostic_Warning()
    {
        var (client, server) = CreateClient("");
        server.Setup(commands => commands.InfoAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("diagnostics timed out"));
        var logger = new CapturingLogger();

        await RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object, new RedisOptions(), logger, CancellationToken.None);

        await Assert.That(logger.Messages[0]).Contains("eviction safety is unknown");
    }

    [Test]
    public async Task Startup_Cancellation_Is_Not_Suppressed()
    {
        var (client, server) = CreateClient("");
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();
        server.Setup(commands => commands.InfoAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException(cancellation.Token));

        await Assert.That(() => RedisMemoryDiagnostics.CheckMemoryPolicyAsync(client.Object,
                new RedisOptions(), new CapturingLogger(), cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Both_Factories_Enforce_FailFast(bool artifacts, bool missingConfiguration)
    {
        var (client, _) = CreateClient(missingConfiguration ? "" : "maxmemory:1024\nmaxmemory_policy:volatile-lru");
        await using var provider = new RedisConnectionProvider(client.Object);
        var options = Microsoft.Extensions.Options.Options.Create(new RedisOptions { FailOnUnsafeMemoryPolicy = true });
        var distributed = Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "memory-check" });

        if (artifacts)
        {
            var factory = new RedisDistributedArtifactStoreFactory(options, distributed, provider);
            await Assert.That(async () => { await factory.CreateAsync(CancellationToken.None); }).Throws<InvalidOperationException>();
        }
        else
        {
            var factory = new RedisDistributedCoordinatorFactory(options, provider, distributed);
            await Assert.That(async () => { await factory.CreateWorkerAsync(CancellationToken.None); }).Throws<InvalidOperationException>();
        }
    }

    [Test]
    [Arguments(100, 103, "same", "same", "delta during download: 3")]
    [Arguments(100, 100, "same", "same", "delta during download: 0")]
    [Arguments(100, 99, "same", "same", "delta unavailable")]
    [Arguments(100, 103, "old", "new", "delta unavailable")]
    public async Task Incomplete_Download_Retains_Integrity_Failure_And_Reports_Observed_Delta(
        int before, int after, string beforeRun, string afterRun, string expected)
    {
        await using var redis = await FakeRedis.StartAsync();
        var (client, server) = CreateClient("");
        server.SetupSequence(commands => commands.InfoAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync($"run_id:{beforeRun}\nevicted_keys:{before}")
            .ReturnsAsync($"run_id:{afterRun}\nevicted_keys:{after}");
        client.Setup(connection => connection.GetBytesAsync(It.IsAny<RespireKey>(), It.IsAny<CancellationToken>()))
            .Returns((RespireKey key, CancellationToken token) => redis.Client.GetBytesAsync(key, token));
        client.SetupGet(connection => connection.Strings).Returns(redis.Client.Strings);
        var keys = new RedisKeyBuilder("memory-test", "run");
        // A missing second chunk simulates TTL expiry, eviction, or deletion during download.
        await redis.Client.SetAsync(keys.ArtifactChunk("partial", 0), (RespireValue) new byte[] { 1, 2 });
        var store = new RedisDistributedArtifactStore(client.Object, keys, new RedisOptions());

        var failure = await Assert.That(async () => { await store.DownloadAsync(Reference("partial", 4), CancellationToken.None); })
            .Throws<InvalidOperationException>();
        await Assert.That(failure!.Message).Contains("size mismatch: expected 4 bytes but got 2 bytes.");
        await Assert.That(failure!.Message).Contains(expected);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Missing_Artifact_Reports_Delta_Without_Attributing_Cause(bool denied)
    {
        var (client, server) = CreateClient("run_id:server\nevicted_keys:50");
        if (denied)
        {
            server.Setup(commands => commands.InfoAsync(null, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RespireServerException("NOPERM"));
        }

        await using var redis = await FakeRedis.StartAsync();
        client.SetupGet(connection => connection.Strings).Returns(redis.Client.Strings);
        var store = new RedisDistributedArtifactStore(client.Object, new RedisKeyBuilder("memory-test", "run"), new RedisOptions());

        var failure = await Assert.That(async () => { await store.DownloadAsync(Reference("missing", 4), CancellationToken.None); })
            .Throws<InvalidOperationException>();
        await Assert.That(failure!.Message).Contains("not found in Redis.");
        await Assert.That(failure!.Message).Contains(denied ? "delta unavailable" : "does not prove");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Both_Factories_Warn_And_Continue_By_Default(bool artifacts)
    {
        var (client, _) = CreateClient("maxmemory:1024\nmaxmemory_policy:volatile-lru");
        await using var provider = new RedisConnectionProvider(client.Object);
        var options = Microsoft.Extensions.Options.Options.Create(new RedisOptions());
        var distributed = Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "memory-check" });

        if (artifacts)
        {
            var logger = new CapturingLogger<RedisDistributedArtifactStore>();
            var factory = new RedisDistributedArtifactStoreFactory(options, distributed, provider, logger);
            await factory.CreateAsync(CancellationToken.None);
            await Assert.That(logger.Messages).HasSingleItem();
        }
        else
        {
            var logger = new CapturingLogger<RedisDistributedCoordinator>();
            var factory = new RedisDistributedCoordinatorFactory(options, provider, distributed, logger);
            var coordinator = await factory.CreateWorkerAsync(CancellationToken.None);
            await using var lifetime = (IAsyncDisposable) coordinator;
            await Assert.That(logger.Messages).HasSingleItem();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Diagnostic_Failure_After_Short_Single_Value_Does_Not_Mask_Integrity_Failure(bool cancel)
    {
        var (client, server) = CreateClient("run_id:server\nevicted_keys:50");
        using var cancellation = new CancellationTokenSource();
        var reads = 0;
        server.Setup(commands => commands.InfoAsync(null, It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                if (++reads == 1)
                {
                    return ValueTask.FromResult("run_id:server\nevicted_keys:50");
                }

                if (cancel)
                {
                    cancellation.Cancel();
                    return ValueTask.FromCanceled<string>(cancellation.Token);
                }

                return ValueTask.FromException<string>(new RespireServerException("NOPERM"));
            });
        client.Setup(connection => connection.GetBytesAsync(It.IsAny<RespireKey>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([1, 2]);
        var store = new RedisDistributedArtifactStore(client.Object, new RedisKeyBuilder("memory-test", "run"), new RedisOptions());

        var failure = await Assert.That(async () => { await store.DownloadAsync(Reference("short", 4), cancellation.Token); })
            .Throws<InvalidOperationException>();
        await Assert.That(failure!.Message).Contains("size mismatch").And.Contains("delta unavailable");
    }

    [Test]
    [Arguments("evicted_keys:100", "run_id:server\nevicted_keys:103")]
    [Arguments("run_id:server\nevicted_keys:100", "run_id:server")]
    [Arguments("run_id:server\nevicted_keys:100", "run_id:server\nevicted_keys:invalid")]
    public async Task Incomplete_Snapshots_Never_Report_A_Cumulative_Counter_As_A_Delta(string before, string after)
    {
        var (client, server) = CreateClient("");
        server.SetupSequence(commands => commands.InfoAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(before).ReturnsAsync(after);
        var snapshot = await RedisMemoryDiagnostics.ReadEvictionSnapshotAsync(client.Object, CancellationToken.None);

        var diagnostic = await RedisMemoryDiagnostics.DescribeEvictionsAsync(client.Object, snapshot, CancellationToken.None);

        await Assert.That(diagnostic).Contains("delta unavailable");
    }

    private static ArtifactReference Reference(string id, long size) => new()
    {
        ArtifactId = id,
        Name = "artifact",
        ModuleId = new ModuleId("module"),
        SizeBytes = size,
        UploadedAt = DateTimeOffset.UtcNow,
    };

    private static (Mock<IRespireClient> Client, Mock<IRespireServerCommands> Server) CreateClient(string info)
    {
        var server = new Mock<IRespireServerCommands>();
        server.Setup(commands => commands.InfoAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync(info);
        var client = new Mock<IRespireClient> { DefaultValue = DefaultValue.Mock };
        client.SetupGet(connection => connection.Server).Returns(server.Object);
        client.Setup(connection => connection.GetBytesAsync(It.IsAny<RespireKey>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?) null);
        return (client, server);
    }

    private sealed class CapturingLogger<T> : CapturingLogger, ILogger<T>;

    private class CapturingLogger : ILogger
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Messages.Add(formatter(state, exception));
            }
        }
    }
}
