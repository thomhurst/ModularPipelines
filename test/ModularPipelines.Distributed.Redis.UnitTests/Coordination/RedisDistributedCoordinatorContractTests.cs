using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.Testing.Distributed;
using Respire;
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
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public Task Lease_Expiration_Budget_Fails_Module(int budget) =>
        RunAsync((coordinator, _, _, _) => DistributedCoordinatorContract.LeaseExpirationBudgetFailsModuleAsync(coordinator, budget),
            1, null, budget: budget);

    [Test]
    public Task Lease_Expiration_Recovery_Succeeds() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.LeaseExpirationRecoverySucceedsAsync(coordinator));

    [Test]
    public Task Reads_All_Pages_Of_Leases_Workers_And_Statuses() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            const int count = 600;
            using var batch = client.CreateBatch();
            for (var index = 0; index < count; index++)
            {
                var workerId = new WorkerId($"worker-{index}");
                var registration = new WorkerRegistration
                {
                    WorkerId = workerId,
                    Capabilities = [],
                    RegisteredAt = DateTimeOffset.UtcNow,
                };
                var status = new WorkerStatus { WorkerId = workerId, IsFinal = index % 3 == 1 };
                var assignment = DistributedCoordinatorContract.CreateAssignment($"Module.{index}");
                var member = RedisQueueMember.Create("0123456789abcdef", assignment, System.Text.Json.JsonSerializer.Serialize(assignment));
                var lease = new { LeaseId = $"lease-{index}", WorkerId = workerId.Value, Member = member, ExpiresAt = "9999999999999" };
                batch.Hashes.Set(keys.Workers, workerId.Value, (RespireValue) System.Text.Json.JsonSerializer.Serialize(registration));
                batch.Hashes.Set(keys.WorkerStatuses, workerId.Value, (RespireValue) System.Text.Json.JsonSerializer.Serialize(status));
                batch.Hashes.Set(keys.Leases, assignment.ModuleId.Value, (RespireValue) System.Text.Json.JsonSerializer.Serialize(lease));
                if (index % 3 == 0)
                {
                    batch.Hashes.Set(keys.WorkerHeartbeats, workerId.Value, (RespireValue) "1");
                }
            }

            await batch.ExecuteAsync();
            var leases = await coordinator.GetActiveLeasesAsync(CancellationToken.None);
            var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None);
            var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

            await Assert.That(leases.Select(lease => lease.Assignment.ModuleId.Value))
                .IsEquivalentTo(Enumerable.Range(0, count).Select(index => $"Module.{index}"));
            await Assert.That(statuses.Select(status => status.WorkerId.Value))
                .IsEquivalentTo(Enumerable.Range(0, count).Select(index => $"worker-{index}"));
            await Assert.That(workers.Select(worker => worker.WorkerId.Value))
                .IsEquivalentTo(Enumerable.Range(0, count).Where(index => index % 3 != 2).Select(index => $"worker-{index}"));
        });

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
    public Task Completion_Closes_Registration() =>
        RunContractAsync((coordinator, _) => DistributedCoordinatorContract.CompletionClosesRegistrationAsync(coordinator));

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
    public Task Server_Supports_Hash_Field_Expiry() =>
        RunAsync((_, client, keys) => RedisDistributedCoordinator.EnsureServerSupportAsync(client, keys, CancellationToken.None));

    [Test]
    public Task Expired_Worker_Heartbeat_Leaves_Only_Final_Workers_Registered() =>
        RunAsync(
            async (coordinator, client, keys, _) =>
            {
                var live = new WorkerRegistration { WorkerId = new WorkerId("contract-live"), RunId = "contract", Capabilities = [], RegisteredAt = DateTimeOffset.UtcNow };
                var finished = new WorkerRegistration { WorkerId = new WorkerId("contract-finished"), RunId = "contract", Capabilities = [], RegisteredAt = DateTimeOffset.UtcNow };
                await coordinator.RegisterWorkerAsync(live, CancellationToken.None);
                await coordinator.RegisterWorkerAsync(finished, CancellationToken.None);
                await coordinator.SendHeartbeatAsync(
                    new WorkerStatus { WorkerId = finished.WorkerId, RunId = "contract", IsFinal = true },
                    CancellationToken.None);

                // The heartbeat fields expire with the worker timeout, so neither worker stays live.
                await WaitUntilAsync(async () => await client.Hashes.CountAsync(keys.WorkerHeartbeats) == 0);

                var registered = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
                await Assert.That(registered.Select(worker => worker.WorkerId)).IsEquivalentTo([finished.WorkerId]);
            },
            readySignalCount: 1,
            workerTimeout: TimeSpan.FromMilliseconds(300));

    [Test]
    public Task Heartbeat_Refreshes_Run_Key_Expiry() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(
                DistributedCoordinatorContract.CreateAssignment("Contract.Ttl"),
                CancellationToken.None);
            await client.ExpireAsync(keys.WorkQueue, TimeSpan.FromSeconds(5));

            await coordinator.SendHeartbeatAsync(
                new WorkerStatus { WorkerId = new WorkerId("contract-worker") },
                CancellationToken.None);

            var ttl = await client.Keys.ExpiryAsync(keys.WorkQueue);
            await Assert.That(ttl.TimeToLive!.Value).IsGreaterThan(TimeSpan.FromSeconds(30));
        });

    [Test]
    public Task Wait_Recovers_A_Result_Whose_Notification_Was_Missed() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var moduleId = new ModuleId("Contract.MissedNotification");
            var wait = coordinator.WaitForResultAsync(moduleId, CancellationToken.None);
            await Task.Delay(200);

            // Write the result without publishing a notification, as if it was lost in a reconnect.
            var result = DistributedCoordinatorContract.CreateResult(moduleId.Value, "stored");
            await client.Hashes.SetAsync(keys.Results, moduleId.Value, (RespireValue) System.Text.Json.JsonSerializer.Serialize(result));

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

                await WaitForMasterStateAsync(redis, DistributedMasterState.Lost);

                // A master that signalled completion finished normally, however long ago.
                await redis.SignalCompletionAsync(CancellationToken.None);
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Completed);
            },
            readySignalCount: 1,
            workerTimeout: null,
            masterTimeout: TimeSpan.FromMilliseconds(200));

    [Test]
    public Task Master_Whose_Heartbeat_Expired_Is_Lost() =>
        RunAsync(
            async (coordinator, client, keys, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;
                await redis.SendMasterHeartbeatAsync(CancellationToken.None);

                // The heartbeat expires once the master stops renewing it; the started marker remains.
                await client.DeleteAsync([keys.MasterHeartbeat]);

                await Assert.That(await client.ExistsAsync(keys.MasterStarted)).IsTrue();
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Lost);
            },
            readySignalCount: 1,
            workerTimeout: null);

    [Test]
    public Task Master_Heartbeat_Loop_Stops_When_The_Coordinator_Is_Disposed() =>
        RunAsync(
            async (coordinator, client, keys, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;
                redis.StartMasterHeartbeat(TimeSpan.FromMilliseconds(50));
                await WaitUntilAsync(async () => await client.ExistsAsync(keys.MasterHeartbeat));
                await Assert.That(await redis.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Running);

                await redis.DisposeAsync();
                await WaitForMasterStateAsync(redis, DistributedMasterState.Lost);
            },
            readySignalCount: 1,
            workerTimeout: null,
            masterTimeout: TimeSpan.FromMilliseconds(500));

    [Test]
    public Task Run_Cleanup_Deletes_Run_Keys_And_Keeps_The_Completion_Flag() =>
        RunAsync(
            async (coordinator, client, keys, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;
                redis.StartMasterHeartbeat(TimeSpan.FromMilliseconds(50));
                await coordinator.EnqueueModuleAsync(
                    DistributedCoordinatorContract.CreateAssignment("Contract.Cleanup"),
                    CancellationToken.None);
                await coordinator.PublishResultAsync(
                    DistributedCoordinatorContract.CreateResult("Contract.Cleanup", "done"),
                    lease: null,
                    CancellationToken.None);
                await coordinator.SendHeartbeatAsync(
                    new WorkerStatus { WorkerId = new WorkerId("contract-worker"), IsFinal = true },
                    CancellationToken.None);
                await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None);
                await coordinator.SignalCompletionAsync(CancellationToken.None);
                await WaitUntilAsync(async () => await client.ExistsAsync(keys.MasterHeartbeat));

                await redis.CleanupRunAsync(
                    new DistributedRunCleanupContext { RunId = "contract", PipelineSucceeded = true },
                    CancellationToken.None);

                // Cleanup keeps registration closed, even after deleting the worker records.
                await Assert.That(async () => await coordinator.RegisterWorkerAsync(new WorkerRegistration
                {
                    WorkerId = new WorkerId("late-worker"),
                    RegisteredAt = DateTimeOffset.UtcNow,
                    Capabilities = [],
                }, CancellationToken.None))
                    .Throws<DistributedRunCompletedException>();

                // A heartbeat sent after cleanup would recreate the master keys.
                await Task.Delay(200);
                foreach (var key in keys.RunCleanupKeys)
                {
                    await Assert.That(await client.ExistsAsync(key)).IsFalse();
                }

                await Assert.That(await client.ExistsAsync(keys.CompletionFlag)).IsTrue();
                await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Completed);
            },
            readySignalCount: 1,
            workerTimeout: null);

    [Test]
    public Task Run_Cleanup_Reports_A_Completed_Master_When_Signalling_Completion_Failed() =>
        RunAsync(
            async (coordinator, client, keys, _) =>
            {
                var redis = (RedisDistributedCoordinator) coordinator;
                await redis.SendMasterHeartbeatAsync(CancellationToken.None);

                // No completion signal: as if SignalCompletionAsync failed before cleanup.
                await redis.CleanupRunAsync(
                    new DistributedRunCleanupContext { RunId = "contract", PipelineSucceeded = false },
                    CancellationToken.None);

                await Assert.That(await client.ExistsAsync(keys.MasterStarted)).IsFalse();
                await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None))
                    .IsEqualTo(DistributedMasterState.Completed);
            },
            readySignalCount: 1,
            workerTimeout: null);

    [Test]
    public Task Lease_Reconciliation_Before_Expiry() =>
        RunAsync((coordinator, _, _) =>
            ModularPipelines.TestHelpers.LeaseReconciliationChecks.ReconnectBeforeExpiryAsync(coordinator, coordinator));

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public Task Lease_Reconciliation_After_Reassignment(bool sameWorker) =>
        RunAsync((coordinator, _, _) =>
            ModularPipelines.TestHelpers.LeaseReconciliationChecks.ReconnectAfterReassignmentAsync(
                coordinator, () => Task.Delay(DistributedCoordinatorContract.LeaseTimeout * 3), sameWorker));

    private static Task WaitForMasterStateAsync(RedisDistributedCoordinator redis, DistributedMasterState expected) =>
        WaitUntilAsync(async () => await redis.GetMasterStateAsync(CancellationToken.None) == expected);

    private static async Task WaitUntilAsync(Func<Task<bool>> condition)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        while (!await condition())
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50), timeout.Token);
        }
    }

    private static Task RunContractAsync(
        Func<IDistributedMasterCoordinator, Task, Task> contract,
        int readySignalCount = 1,
        TimeSpan? workerTimeout = null) =>
        RunAsync((coordinator, _, _, ready) => contract(coordinator, ready), readySignalCount, workerTimeout);

    private static Task RunAsync(Func<IDistributedMasterCoordinator, IRespireClient, RedisKeyBuilder, Task> test) =>
        RunAsync((coordinator, client, keys, _) => test(coordinator, client, keys), 1, null);

    private static async Task RunAsync(
        Func<IDistributedMasterCoordinator, IRespireClient, RedisKeyBuilder, Task, Task> test,
        int readySignalCount,
        TimeSpan? workerTimeout,
        TimeSpan? masterTimeout = null,
        int budget = 2)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis contract tests.");
        }

#pragma warning disable RESP010 // Each isolated contract test owns one client for all operations in its run.
        await using var connection = await RespireClient.ConnectAsync(connectionString);
#pragma warning restore RESP010
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
            connection,
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
                MaxModuleLeaseExpirations = budget,
                ModuleResultTimeout = TimeSpan.FromSeconds(30),
                MasterTimeout = masterTimeout ?? TimeSpan.FromSeconds(30),
            });

        await test(coordinator, connection, keys, ready.Task);
    }
}
