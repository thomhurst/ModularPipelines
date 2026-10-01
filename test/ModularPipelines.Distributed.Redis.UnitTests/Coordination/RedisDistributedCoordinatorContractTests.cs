using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.TestHelpers.Distributed;
using StackExchange.Redis;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

/// <summary>
/// Runs the shared coordinator contract against a real Redis server. Set
/// <c>MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING</c> to run them.
/// </summary>
public class RedisDistributedCoordinatorContractTests
{
    private const string ConnectionStringVariable = "MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING";

    [Test]
    public Task Enqueue_And_Dequeue_RoundTrips() =>
        RunContractAsync(DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync);

    [Test]
    public Task Publish_Unblocks_Wait_And_RoundTrips_Result() =>
        RunContractAsync(DistributedCoordinatorContract.ResultRoundTripsAfterWaitStartsAsync);

    [Test]
    public Task First_Published_Result_Is_Final() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.FirstPublishedResultIsFinalAsync(coordinator));

    [Test]
    public Task Completion_Signal_Unblocks_Pending_Dequeue() =>
        RunContractAsync(DistributedCoordinatorContract.CompletionUnblocksPendingDequeueAsync);

    [Test]
    public Task Canceled_Dequeue_Throws() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.CanceledDequeueThrowsAsync(coordinator));

    [Test]
    public Task Cancellation_Signal_Unblocks_Worker_Observer() =>
        RunContractAsync(DistributedCoordinatorContract.CancellationUnblocksWorkerObserverAsync);

    [Test]
    public Task Pipeline_Failure_Only_Releases_AlwaysRun_Work() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.PipelineFailureOnlyReleasesAlwaysRunWorkAsync(coordinator));

    [Test]
    public Task Stop_Closes_Dequeue() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.StopClosesDequeueAsync(coordinator));

    [Test]
    public Task Withdraw_Removes_Queued_Assignment() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.WithdrawRemovesQueuedAssignmentAsync(coordinator));

    [Test]
    public Task Expired_Lease_Is_Requeued() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.ExpiredLeaseIsRequeuedAsync(coordinator));

    [Test]
    public Task Heartbeat_Renews_Lease() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.HeartbeatRenewsLeaseAsync(coordinator));

    [Test]
    public Task Publishing_Releases_Lease() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.PublishingReleasesLeaseAsync(coordinator));

    [Test]
    public Task Heartbeat_Keeps_Worker_Registration_Live() =>
        RunContractAsync(
            (coordinator, _) => DistributedCoordinatorContract.WorkerHeartbeatKeepsRegistrationLiveAsync(coordinator),
            workerTimeout: TimeSpan.FromSeconds(30));

    [Test]
    public Task Duplicate_Live_Worker_Is_Rejected() =>
        RunContractAsync(
            (coordinator, _) => DistributedCoordinatorContract.DuplicateLiveWorkerIsRejectedAsync(coordinator),
            workerTimeout: TimeSpan.FromSeconds(30));

    [Test]
    public Task Claim_Prefers_Scarce_Capability_Work() =>
        RunContractAsync(
            (coordinator, _) => DistributedCoordinatorContract.ClaimPrefersScarceCapabilityWorkAsync(coordinator),
            workerTimeout: TimeSpan.FromSeconds(30));

    [Test]
    public Task Claim_Prefers_Priority_Then_Critical_Path() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.ClaimPrefersPriorityThenCriticalPathAsync(coordinator));

    [Test]
    public Task Claim_Matches_Alternative_Capabilities() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.ClaimMatchesAlternativeCapabilitiesAsync(coordinator));

    [Test]
    public Task Final_Metrics_Keep_Worker_Registration_After_Heartbeat_Expires() =>
        RunContractAsync(
            (coordinator, _) => DistributedCoordinatorContract.FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
                coordinator,
                TimeSpan.FromMilliseconds(250)),
            workerTimeout: TimeSpan.FromMilliseconds(100));

    [Test]
    public Task Cancelling_One_Observer_Leaves_Concurrent_Observer_Subscribed() =>
        RunContractAsync(
            DistributedCoordinatorContract.CancellationKeepsConcurrentObserverSubscribedAsync,
            readySignalCount: 2);

    [Test]
    public Task Heartbeat_Refreshes_Run_Key_Expiry() =>
        RunAsync(async (coordinator, database, keys) =>
        {
            await coordinator.EnqueueModuleAsync(
                DistributedCoordinatorContract.CreateAssignment("Contract.Ttl"),
                CancellationToken.None);
            await database.KeyExpireAsync(keys.WorkQueue, TimeSpan.FromSeconds(5));

            await coordinator.SendHeartbeatAsync(
                new WorkerStatus { WorkerId = new WorkerId("contract-worker") },
                CancellationToken.None);

            var ttl = await database.KeyTimeToLiveAsync(keys.WorkQueue);
            await Assert.That(ttl!.Value).IsGreaterThan(TimeSpan.FromSeconds(30));
        });

    [Test]
    public Task Wait_Recovers_A_Result_Whose_Notification_Was_Missed() =>
        RunAsync(async (coordinator, database, keys) =>
        {
            var moduleId = new ModuleId("Contract.MissedNotification");
            var wait = coordinator.WaitForResultAsync(moduleId, CancellationToken.None);
            await Task.Delay(200);

            // Write the result without publishing a notification, as if it was lost in a reconnect.
            var result = DistributedCoordinatorContract.CreateResult(moduleId.Value, "stored");
            await database.HashSetAsync(keys.Results, moduleId.Value, System.Text.Json.JsonSerializer.Serialize(result));

            var received = await wait.WaitAsync(RedisDistributedCoordinator.PollInterval * 3);
            await Assert.That(received.Payload).IsEqualTo(result.Payload);
        });

    [Test]
    public Task Master_State_Follows_Heartbeat_And_Completion() =>
        RunAsync(
            async (coordinator, _, _, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;

                // A master that has not sent a heartbeat yet has not started; it is not lost.
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Running);

                await redis.SendMasterHeartbeatAsync(CancellationToken.None);
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Running);

                await Task.Delay(TimeSpan.FromMilliseconds(500));
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Lost);

                // A master that signalled completion finished normally, however long ago.
                await redis.SignalCompletionAsync(CancellationToken.None);
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Completed);
            },
            readySignalCount: 1,
            workerTimeout: null,
            masterTimeout: TimeSpan.FromMilliseconds(200));

    [Test]
    public Task Master_Heartbeat_Loop_Stops_When_The_Coordinator_Is_Disposed() =>
        RunAsync(
            async (coordinator, _, _, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;
                redis.StartMasterHeartbeat(TimeSpan.FromMilliseconds(50));
                await Task.Delay(TimeSpan.FromMilliseconds(400));
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Running);

                await redis.DisposeAsync();
                await Task.Delay(TimeSpan.FromSeconds(3));
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Lost);
            },
            readySignalCount: 1,
            workerTimeout: null,
            masterTimeout: TimeSpan.FromSeconds(2));

    private static Task RunContractAsync(
        Func<IDistributedMasterCoordinator, Task, Task> contract,
        int readySignalCount = 1,
        TimeSpan? workerTimeout = null) =>
        RunAsync((coordinator, _, _, ready) => contract(coordinator, ready), readySignalCount, workerTimeout);

    private static Task RunAsync(Func<IDistributedMasterCoordinator, IDatabase, RedisKeyBuilder, Task> test) =>
        RunAsync((coordinator, database, keys, _) => test(coordinator, database, keys), 1, null);

    private static async Task RunAsync(
        Func<IDistributedMasterCoordinator, IDatabase, RedisKeyBuilder, Task, Task> test,
        int readySignalCount,
        TimeSpan? workerTimeout,
        TimeSpan? masterTimeout = null)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis contract tests.");
        }

        using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
        var runId = Guid.NewGuid().ToString("N");
        var options = new RedisOptions
        {
            ConnectionString = connectionString,
            TimeToLive = TimeSpan.FromMinutes(1),
            KeyPrefix = "modpipe-contract",
        };
        var keys = new RedisKeyBuilder(options.KeyPrefix, runId);
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
                WorkerTimeout = workerTimeout ?? DistributedCoordinatorContract.LeaseTimeout,
                ModuleResultTimeout = TimeSpan.FromSeconds(30),
                MasterTimeout = masterTimeout ?? TimeSpan.FromMinutes(1),
            });

        await test(coordinator, connection.GetDatabase(), keys, ready.Task);
    }
}
