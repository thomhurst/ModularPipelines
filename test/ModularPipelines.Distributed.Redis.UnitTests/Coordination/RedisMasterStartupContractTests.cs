using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.Testing.Distributed;
using Respire;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisMasterStartupContractTests
{
    // Include absolute expiry times so a rejected startup cannot silently extend the old run's lifetime.
    private static readonly RespireScript SnapshotScript = RespireScript.Create("""
        local snapshot = {}
        for i, key in ipairs(KEYS) do
            local value = redis.call('DUMP', key)
            snapshot[i] = {value and redis.sha1hex(value) or '', redis.call('PEXPIRETIME', key)}
        end
        return cjson.encode(snapshot)
        """);

    [Test]
    public Task Completed_Run_Is_Rejected_Without_Changing_Its_State() =>
        RunAsync(async (factory, client, keys, options) =>
        {
            await using var master = (RedisDistributedCoordinator) await factory.CreateMasterAsync(CancellationToken.None);
            await master.SignalCompletionAsync(CancellationToken.None);
            await master.CleanupRunAsync(
                new DistributedRunCleanupContext { RunId = options.RunId, PipelineSucceeded = true },
                CancellationToken.None);

            await AssertReusedRunAsync(factory, client, keys);
        });

    [Test]
    public Task Crashed_Run_With_Only_Results_Is_Rejected_Without_Changing_Its_State() =>
        RunAsync(async (factory, client, keys, _) =>
        {
            var result = DistributedCoordinatorContract.CreateResult("Previous.Module", "old result");
            await client.Hashes.SetAsync(keys.Results, result.ModuleId.Value,
                (RespireValue) System.Text.Json.JsonSerializer.Serialize(result));
            await client.ExpireAsync(keys.Results, TimeSpan.FromSeconds(45));

            await AssertReusedRunAsync(factory, client, keys);
        });

    [Test]
    public Task Started_Run_Without_A_Heartbeat_Is_Rejected_Without_Changing_Its_State() =>
        RunAsync(async (factory, client, keys, _) =>
        {
            await client.SetAsync(keys.MasterStarted, (RespireValue) "1", TimeSpan.FromSeconds(45));

            await AssertReusedRunAsync(factory, client, keys);
        });

    [Test]
    public Task Simultaneous_Masters_Reserve_Exactly_Once() =>
        RunAsync(async (factory, client, keys, _) =>
        {
            var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            async Task<bool> TryStartAsync()
            {
                await start.Task;
                try
                {
                    await using var master = (RedisDistributedCoordinator) await factory.CreateMasterAsync(CancellationToken.None);
                    return true;
                }
                catch (InvalidOperationException exception) when (exception.Message.Contains("RunId already used", StringComparison.Ordinal))
                {
                    return false;
                }
            }

            var first = TryStartAsync();
            var second = TryStartAsync();
            start.SetResult();
            var results = await Task.WhenAll(first, second);

            await Assert.That(results.Count(success => success)).IsEqualTo(1);
            await Assert.That(await client.ExistsAsync(keys.MasterStarted)).IsTrue();
            await Assert.That(await client.ExistsAsync(keys.MasterHeartbeat)).IsTrue();
            await AssertReusedRunAsync(factory, client, keys);
        });

    [Test]
    public Task Workers_Can_Register_Before_The_First_Master_And_Execute_Work() =>
        RunAsync(async (factory, client, keys, options) =>
        {
            await using var worker = (RedisDistributedCoordinator) await factory.CreateWorkerAsync(CancellationToken.None);
            var registration = new WorkerRegistration
            {
                WorkerId = new WorkerId("early-worker"),
                RunId = options.RunId,
                Capabilities = [],
                RegisteredAt = DateTimeOffset.UtcNow,
            };
            await worker.RegisterWorkerAsync(registration, CancellationToken.None);
            await Assert.That(await client.ExistsAsync(keys.MasterStarted)).IsFalse();

            await using var master = (RedisDistributedCoordinator) await factory.CreateMasterAsync(CancellationToken.None);
            await Assert.That((await master.GetRegisteredWorkersAsync(CancellationToken.None)).Select(value => value.WorkerId))
                .IsEquivalentTo([registration.WorkerId]);
            var assignment = DistributedCoordinatorContract.CreateAssignment("Fresh.Module");
            await master.EnqueueModuleAsync(assignment, CancellationToken.None);
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), timeout.Token);
            await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(assignment.ModuleId);
        });

    [Test]
    public Task Fresh_RunId_Is_Independent_Of_A_Completed_Run() =>
        RunAsync(async (_, client, keys, options) =>
        {
            await client.SetAsync(keys.CompletionFlag, (RespireValue) "1", TimeSpan.FromSeconds(45));
            var previous = await SnapshotAsync(client, keys);
            var redisOptions = new RedisOptions { KeyPrefix = "modpipe-startup-contract", TimeToLive = TimeSpan.FromMinutes(2) };
            await using var connections = new RedisConnectionProvider(client);
            var freshOptions = options with { RunId = Guid.NewGuid().ToString("N") };
            var factory = new RedisDistributedCoordinatorFactory(Microsoft.Extensions.Options.Options.Create(redisOptions), connections, Microsoft.Extensions.Options.Options.Create(freshOptions));
            var freshKeys = new RedisKeyBuilder(redisOptions.KeyPrefix, freshOptions.RunId);
            try
            {
                await using var master = (RedisDistributedCoordinator) await factory.CreateMasterAsync(CancellationToken.None);
                await Assert.That(await client.ExistsAsync(freshKeys.MasterStarted)).IsTrue();
                await Assert.That(await client.ExistsAsync(freshKeys.MasterHeartbeat)).IsTrue();
                await Assert.That(await SnapshotAsync(client, keys)).IsEqualTo(previous);
            }
            finally
            {
                await client.DeleteAsync([.. freshKeys.ScriptKeys.Select(key => (RespireKey) key)]);
            }
        });

    private static async Task AssertReusedRunAsync(
        RedisDistributedCoordinatorFactory factory, IRespireClient client, RedisKeyBuilder keys)
    {
        var previous = await SnapshotAsync(client, keys);
        await Assert.That(async () =>
            {
                await using var unexpected = (RedisDistributedCoordinator) await factory.CreateMasterAsync(CancellationToken.None);
            })
            .Throws<InvalidOperationException>()
            .WithMessageContaining("RunId already used");
        await Assert.That(await SnapshotAsync(client, keys)).IsEqualTo(previous);
    }

    private static async Task<string?> SnapshotAsync(IRespireClient client, RedisKeyBuilder keys) =>
        await client.Scripts.ExecuteStringAsync(SnapshotScript, [.. keys.ScriptKeys.Select(key => (RespireKey) key)]);

    private static async Task RunAsync(
        Func<RedisDistributedCoordinatorFactory, IRespireClient, RedisKeyBuilder, DistributedOptions, Task> test)
    {
        var connectionString = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException("Set MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING to run real Redis contract tests.");
        }

#pragma warning disable RESP010 // Each isolated contract test owns one client for all operations in its run.
        await using var client = await RespireClient.ConnectAsync(connectionString);
#pragma warning restore RESP010
        await using var connections = new RedisConnectionProvider(client);
        var redisOptions = new RedisOptions { KeyPrefix = "modpipe-startup-contract", TimeToLive = TimeSpan.FromMinutes(2) };
        var options = new DistributedOptions
        {
            RunId = Guid.NewGuid().ToString("N"),
            WorkerHeartbeatInterval = TimeSpan.FromHours(1),
            ModuleResultTimeout = TimeSpan.FromSeconds(30),
        };
        var keys = new RedisKeyBuilder(redisOptions.KeyPrefix, options.RunId);
        var factory = new RedisDistributedCoordinatorFactory(Microsoft.Extensions.Options.Options.Create(redisOptions), connections, Microsoft.Extensions.Options.Options.Create(options));
        try
        {
            await test(factory, client, keys, options);
        }
        finally
        {
            await client.DeleteAsync([.. keys.ScriptKeys.Select(key => (RespireKey) key)]);
        }
    }
}
