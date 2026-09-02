using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.TestHelpers.Distributed;
using StackExchange.Redis;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisDistributedCoordinatorContractTests
{
    private const string ConnectionStringVariable = "MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING";

    [Test]
    public Task Enqueue_And_Dequeue_RoundTrips()
    {
        return RunContractAsync(DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync);
    }

    [Test]
    public Task Publish_Unblocks_Wait_And_RoundTrips_Result()
    {
        return RunContractAsync(DistributedCoordinatorContract.ResultRoundTripsAfterWaitStartsAsync);
    }

    [Test]
    public Task Completion_Signal_Unblocks_Pending_Dequeue()
    {
        return RunContractAsync(DistributedCoordinatorContract.CompletionUnblocksPendingDequeueAsync);
    }

    [Test]
    public Task Cancellation_Signal_Unblocks_Worker_Observer()
    {
        return RunContractAsync(DistributedCoordinatorContract.CancellationUnblocksWorkerObserverAsync);
    }

    [Test]
    public Task Heartbeat_Keeps_Worker_Registration_Live()
    {
        return RunContractAsync((coordinator, _) =>
            DistributedCoordinatorContract.WorkerHeartbeatKeepsRegistrationLiveAsync(coordinator));
    }

    [Test]
    public Task Final_Metrics_Keep_Worker_Registration_After_Heartbeat_Expires()
    {
        return RunContractAsync((coordinator, _) =>
            DistributedCoordinatorContract.FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
                coordinator,
                TimeSpan.FromMilliseconds(250)),
            workerTimeout: TimeSpan.FromMilliseconds(100));
    }

    [Test]
    public Task Cancelling_One_Observer_Leaves_Concurrent_Observer_Subscribed()
    {
        return RunContractAsync(
            DistributedCoordinatorContract.CancellationKeepsConcurrentObserverSubscribedAsync,
            readySignalCount: 2);
    }

    private static async Task RunContractAsync(
        Func<IDistributedMasterCoordinator, Task, Task> contract,
        int readySignalCount = 1,
        TimeSpan? workerTimeout = null)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis contract tests.");
        }

        using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
        var options = new RedisDistributedOptions
        {
            ConnectionString = connectionString,
            KeyExpirationSeconds = 60,
            KeyPrefix = "modpipe-contract",
            RunIdentifier = Guid.NewGuid().ToString("N"),
        };
        var keys = new RedisKeyBuilder(options.KeyPrefix, options.RunIdentifier!);
        var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var readySignals = 0;
        var coordinator = new RedisDistributedCoordinator(
            connection.GetDatabase(),
            connection.GetSubscriber(),
            keys,
            options,
            () =>
            {
                if (Interlocked.Increment(ref readySignals) >= readySignalCount)
                {
                    ready.TrySetResult();
                }
            },
            new DistributedOptions
            {
                WorkerTimeout = workerTimeout ?? TimeSpan.FromSeconds(30),
            });

        await contract(coordinator, ready.Task);
    }
}
