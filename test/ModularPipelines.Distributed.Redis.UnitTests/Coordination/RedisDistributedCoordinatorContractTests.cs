using System.Text.Json;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.Testing.Distributed;
using Moq;
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
    [Arguments(1, false)]
    [Arguments(5, false)]
    [Arguments(1, true)]
    [Arguments(5, true)]
    public Task Lost_Claim_Response_Retries_With_The_Same_Token_Unless_Cancelled(int lostResponses, bool cancel) =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("First"), CancellationToken.None);
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("Second"), CancellationToken.None);
            using var cancellation = new CancellationTokenSource();
            var calls = 0;
            var claimIds = new List<string>();
            var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
            scripts.Setup(commands => commands.ExecuteStringAsync(
                    It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
                .Returns(async (RespireScript script, RespireKey[] scriptKeys, RespireValue[] arguments, CancellationToken token) =>
                {
                    claimIds.Add(arguments[2].ToString());
                    var result = await client.Scripts.ExecuteStringAsync(script, scriptKeys, arguments, token);
                    if (++calls <= lostResponses)
                    {
                        if (cancel && calls == lostResponses)
                        {
                            // Redis committed the claim, but cancellation abandoned the response.
                            cancellation.Cancel();
                            token.ThrowIfCancellationRequested();
                        }

                        throw new RespireConnectionException("Claim response lost after execution");
                    }

                    return result;
                });
            var connection = new Mock<IRespireClient>(MockBehavior.Strict);
            connection.SetupGet(redis => redis.Scripts).Returns(scripts.Object);
            await using var claimant = new RedisDistributedCoordinator(connection.Object, keys, new RedisOptions());

            if (cancel)
            {
                await Assert.That(async () => await claimant.TryClaimAsync(
                        new WorkerId("worker"), new HashSet<Capability>(), cancellation.Token))
                    .Throws<OperationCanceledException>();
                await Assert.That(calls).IsEqualTo(lostResponses);
                var lease = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
                await ExpireLeaseAsync(client, keys, lease.Assignment.ModuleId);
                await Assert.That(await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None))
                    .Contains(lease.Assignment.ModuleId);
                await Assert.That(await client.Hashes.ExistsAsync(keys.Results, lease.Assignment.ModuleId.Value)).IsFalse();
            }
            else
            {
                var (_, lease) = await claimant.TryClaimAsync(new WorkerId("worker"), new HashSet<Capability>(), CancellationToken.None);
                await Assert.That(lease).IsNotNull();
                await Assert.That(calls).IsGreaterThan(lostResponses);
                await Assert.That(claimIds.Distinct()).Count().IsEqualTo(1);
                await Assert.That((await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single().LeaseId)
                    .IsEqualTo(lease!.LeaseId);
            }
        });

    [Test]
    [Arguments(DistributedCancellationReason.PipelineFailed, false)]
    [Arguments(DistributedCancellationReason.PipelineFailed, true)]
    [Arguments(DistributedCancellationReason.Stopped, false)]
    [Arguments(DistributedCancellationReason.Stopped, true)]
    public Task Lost_Claim_Response_Retry_Observes_Current_Cancellation(
        DistributedCancellationReason reason, bool alwaysRun) =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("First") with { AlwaysRun = alwaysRun };
            await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
            var calls = 0;
            var claimIds = new List<string>();
            var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
            scripts.Setup(commands => commands.ExecuteStringAsync(
                    It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
                .Returns(async (RespireScript script, RespireKey[] scriptKeys, RespireValue[] arguments, CancellationToken token) =>
                {
                    claimIds.Add(arguments[2].ToString());
                    var result = await client.Scripts.ExecuteStringAsync(script, scriptKeys, arguments, token);
                    if (++calls == 1)
                    {
                        await Assert.That(result).IsNotNull();
                        // Failure arrives after Redis claims work but before the worker receives it.
                        await coordinator.BroadcastCancellationAsync(reason, CancellationToken.None);
                        throw new RespireConnectionException("Claim response lost after execution");
                    }

                    return result;
                });
            var connection = new Mock<IRespireClient>(MockBehavior.Strict);
            connection.SetupGet(redis => redis.Scripts).Returns(scripts.Object);
            await using var claimant = new RedisDistributedCoordinator(connection.Object, keys, new RedisOptions());

            var (closed, lease) = await claimant.TryClaimAsync(new WorkerId("worker"), new HashSet<Capability>(), CancellationToken.None);
            var shouldDeliver = alwaysRun && reason == DistributedCancellationReason.PipelineFailed;
            await Assert.That(lease is not null).IsEqualTo(shouldDeliver);
            await Assert.That(closed).IsEqualTo(reason == DistributedCancellationReason.Stopped);
            await Assert.That(calls).IsEqualTo(shouldDeliver ? 3 : 2);
            await Assert.That(claimIds.Distinct()).Count().IsEqualTo(1);
            await Assert.That((await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single().Assignment.ModuleId)
                .IsEqualTo(assignment.ModuleId);
        });

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public Task Lost_Acknowledgement_Response_Returns_The_Received_Assignment(bool executed) =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("First"), CancellationToken.None);
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("Second"), CancellationToken.None);
            var calls = 0;
            var claimIds = new List<string>();
            var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
            scripts.Setup(commands => commands.ExecuteStringAsync(
                    It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
                .Returns(async (RespireScript script, RespireKey[] scriptKeys, RespireValue[] arguments, CancellationToken token) =>
                {
                    claimIds.Add(arguments[2].ToString());
                    // Fail every acknowledgement response, including failures before Redis executes it.
                    if (++calls > 1)
                    {
                        if (executed)
                        {
                            await client.Scripts.ExecuteStringAsync(script, scriptKeys, arguments, token);
                        }

                        // Fail any retry immediately so persistent response loss cannot hang this test.
                        if (calls > 2)
                        {
                            throw new InvalidOperationException("A received assignment must not wait for acknowledgement retries");
                        }

                        throw new RespireConnectionException("Acknowledgement response unavailable");
                    }

                    return await client.Scripts.ExecuteStringAsync(script, scriptKeys, arguments, token);
                });
            var connection = new Mock<IRespireClient>(MockBehavior.Strict);
            connection.SetupGet(redis => redis.Scripts).Returns(scripts.Object);
            await using var claimant = new RedisDistributedCoordinator(connection.Object, keys, new RedisOptions());
            var (_, lease) = await claimant.TryClaimAsync(new WorkerId("worker"), new HashSet<Capability>(), CancellationToken.None);
            await Assert.That(lease).IsNotNull();
            await Assert.That(calls).IsEqualTo(2);
            await Assert.That(claimIds.Distinct()).Count().IsEqualTo(1);
            await Assert.That((await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single().LeaseId)
                .IsEqualTo(lease!.LeaseId);
            // The returned assignment can execute and publish even when acknowledgement was ambiguous.
            await coordinator.PublishResultAsync(
                DistributedCoordinatorContract.CreateResult(lease.Assignment.ModuleId.Value, "done"), lease, CancellationToken.None);
            await Assert.That(await client.Hashes.ExistsAsync(keys.Results, lease.Assignment.ModuleId.Value)).IsTrue();
        });

    [Test]
    public Task Cancellation_After_Claim_Delivery_Does_Not_Discard_The_Assignment() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("Delivered"), CancellationToken.None);
            using var cancellation = new CancellationTokenSource();
            var calls = 0;
            var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
            scripts.Setup(commands => commands.ExecuteStringAsync(
                    It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
                .Returns(async (RespireScript script, RespireKey[] scriptKeys, RespireValue[] arguments, CancellationToken token) =>
                {
                    var result = await client.Scripts.ExecuteStringAsync(script, scriptKeys, arguments, token);
                    if (++calls == 2)
                    {
                        cancellation.Cancel();
                        token.ThrowIfCancellationRequested();
                    }

                    return result;
                });
            var connection = new Mock<IRespireClient>(MockBehavior.Strict);
            connection.SetupGet(redis => redis.Scripts).Returns(scripts.Object);
            await using var claimant = new RedisDistributedCoordinator(connection.Object, keys, new RedisOptions());

            var (_, lease) = await claimant.TryClaimAsync(new WorkerId("worker"), new HashSet<Capability>(), cancellation.Token);
            await Assert.That(lease).IsNotNull();
            await Assert.That(cancellation.IsCancellationRequested).IsTrue();
            await Assert.That(calls).IsEqualTo(2);
            await coordinator.PublishResultAsync(
                DistributedCoordinatorContract.CreateResult(lease!.Assignment.ModuleId.Value, "cancelled"), lease, CancellationToken.None);
            await Assert.That(await client.Hashes.ExistsAsync(keys.Results, lease.Assignment.ModuleId.Value)).IsTrue();
        });

    [Test]
    public Task Retried_Acknowledgements_Do_Not_Extend_An_Unstarted_Lease() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("Unstarted");
            await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
            var claimed = await ClaimAsync(client, keys, "claim-1");
            await AcknowledgeAsync(client, keys, "claim-1");
            var readDeadline = RespireScript.Create("return cjson.decode(redis.call('HGET', KEYS[1], ARGV[1]))['ExpiresAt']");
            var deadline = await client.Scripts.ExecuteStringAsync(readDeadline, [keys.Leases], [assignment.ModuleId.Value]);
            for (var attempt = 0; attempt < 3; attempt++)
            {
                await Task.Delay(20);
                await Assert.That(await AcknowledgeAsync(client, keys, "claim-1")).IsEqualTo(claimed);
                await Assert.That(await client.Scripts.ExecuteStringAsync(readDeadline, [keys.Leases], [assignment.ModuleId.Value]))
                    .IsEqualTo(deadline);
            }

            await ExpireLeaseAsync(client, keys, assignment.ModuleId);
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-1")).IsNull();
            await Assert.That(await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None)).Contains(assignment.ModuleId);
        });

    [Test]
    public Task Acknowledgement_Rejects_Unknown_Expired_Reassigned_Completed_And_Wrong_Worker_Claims() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("First"), CancellationToken.None);
            await Assert.That(await AcknowledgeAsync(client, keys, "unknown")).IsNull();
            var assignment = await ClaimAsync(client, keys, "claim-1");
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-1", "other-worker")).IsNull();
            var original = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
            await ExpireLeaseAsync(client, keys, original.Assignment.ModuleId);
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-1")).IsNull();
            await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);
            await ClaimAsync(client, keys, "claim-2");
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-1")).IsNull();
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-2")).IsEqualTo(assignment);
            var replacement = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
            await coordinator.PublishResultAsync(
                DistributedCoordinatorContract.CreateResult(replacement.Assignment.ModuleId.Value, "done"), replacement, CancellationToken.None);
            await Assert.That(await AcknowledgeAsync(client, keys, "claim-2")).IsNull();
        });

    [Test]
    public Task Retried_Claim_Returns_The_Same_Lease_Without_Claiming_Another_Module() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("First"), CancellationToken.None);
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("Second"), CancellationToken.None);

            var first = await ClaimAsync(client, keys, "claim-1");
            var retry = await ClaimAsync(client, keys, "claim-1");
            await Assert.That(retry).IsEqualTo(first);
            await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Count().IsEqualTo(1);

            // Concurrent slots on one worker need independent claims.
            var second = await ClaimAsync(client, keys, "claim-2");
            await Assert.That(second).IsNotNull();
            await Assert.That(second).IsNotEqualTo(first);
            await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Count().IsEqualTo(2);
        });

    [Test]
    public Task Retried_Expired_Or_Completed_Claim_Does_Not_Take_New_Work() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("First"), CancellationToken.None);
            await ClaimAsync(client, keys, "claim-1");
            var original = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
            await ExpireLeaseAsync(client, keys, original.Assignment.ModuleId);
            await Assert.That(await ClaimAsync(client, keys, "claim-1")).IsNull();
            await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);

            await ClaimAsync(client, keys, "claim-2");
            var replacement = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
            await coordinator.EnqueueModuleAsync(DistributedCoordinatorContract.CreateAssignment("Second"), CancellationToken.None);
            await Assert.That(await ClaimAsync(client, keys, "claim-1")).IsNull();
            await Assert.That((await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single().LeaseId)
                .IsEqualTo(replacement.LeaseId);

            await coordinator.PublishResultAsync(
                DistributedCoordinatorContract.CreateResult(replacement.Assignment.ModuleId.Value, "done"), replacement, CancellationToken.None);
            await Assert.That(await ClaimAsync(client, keys, "claim-2")).IsNull();
            await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).IsEmpty();
            await Assert.That(await ClaimAsync(client, keys, "claim-3")).IsNotNull();
        });

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public Task Unrenewed_Claims_Do_Not_Consume_Or_Reset_The_Expiration_Budget(bool reconcile) =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("Abandoned");
            await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var worker = new WorkerId($"worker-{attempt}");
                await client.Scripts.ExecuteStringAsync(RespireScript.Create(RedisCoordinationScripts.Claim),
                    [.. keys.ScriptKeys.Select(static key => (RespireKey) key)],
                    ["[]", worker.Value, $"claim-{attempt}", 30000, 60000]);
                var lease = (await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Single();
                // Raw claims have no delivery acknowledgement. Heartbeats can acknowledge them too.
                if (attempt is 0 or 4)
                {
                    var status = new WorkerStatus { WorkerId = worker, InFlightModules = [assignment.ModuleId] };
                    if (reconcile)
                    {
                        await coordinator.ReconcileHeartbeatAsync(status, [lease], CancellationToken.None);
                    }
                    else
                    {
                        await coordinator.SendHeartbeatAsync(status, CancellationToken.None);
                    }
                }

                await ExpireLeaseAsync(client, keys, assignment.ModuleId);
                var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);
                await Assert.That(requeued.Count).IsEqualTo(attempt == 4 ? 0 : 1);
                await Assert.That(await client.Hashes.ExistsAsync(keys.Results, assignment.ModuleId.Value))
                    .IsEqualTo(attempt == 4);
            }

            var result = await coordinator.WaitForResultAsync(assignment.ModuleId, CancellationToken.None);
            await Assert.That(result.Payload).Contains("worker-0").And.Contains("worker-4");
            await Assert.That(result.Payload).DoesNotContain("worker-1");
        });

    [Test]
    public Task Delivered_Claims_Count_Toward_The_Budget_Before_The_First_Heartbeat() =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("EarlyCrash");
            await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
            for (var attempt = 0; attempt < 2; attempt++)
            {
                var worker = new WorkerId($"worker-{attempt}");
                var lease = await coordinator.DequeueModuleAsync(worker, new HashSet<Capability>(), CancellationToken.None);
                await Assert.That(lease).IsNotNull();
                // Simulate a crash immediately after delivery, before any periodic heartbeat.
                await ExpireLeaseAsync(client, keys, assignment.ModuleId);
                var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);
                await Assert.That(requeued.Count).IsEqualTo(attempt == 0 ? 1 : 0);
            }

            var result = await coordinator.WaitForResultAsync(assignment.ModuleId, CancellationToken.None);
            await Assert.That(result.Payload).Contains("worker-0").And.Contains("worker-1");
        });

    private static ValueTask<string?> AcknowledgeAsync(
        IRespireClient client, RedisKeyBuilder keys, string claimId, string workerId = "worker") =>
        client.Scripts.ExecuteStringAsync(RespireScript.Create(RedisCoordinationScripts.AcknowledgeClaim),
            [.. keys.ScriptKeys.Select(static key => (RespireKey) key)],
            ["[]", workerId, claimId, 30000, 60000]);

    private static ValueTask<string?> ClaimAsync(IRespireClient client, RedisKeyBuilder keys, string claimId) =>
        client.Scripts.ExecuteStringAsync(RespireScript.Create(RedisCoordinationScripts.Claim),
            [.. keys.ScriptKeys.Select(static key => (RespireKey) key)],
            ["[]", "worker", claimId, 30000, 60000]);

    private static async Task ExpireLeaseAsync(IRespireClient client, RedisKeyBuilder keys, ModuleId moduleId) =>
        await client.Scripts.ExecuteIntegerAsync(RespireScript.Create("""
            local lease = cjson.decode(redis.call('HGET', KEYS[1], ARGV[1]))
            lease['ExpiresAt'] = '0'
            redis.call('HSET', KEYS[1], ARGV[1], cjson.encode(lease))
            return 1
            """), [keys.Leases], [moduleId.Value]);

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
    [Arguments("GPU-Ä", "gpu-ä")]
    [Arguments("gpu-ä", "GPU-Ä")]
    [Arguments("Σ", "ς")]
    [Arguments("𐐀", "𐐨")]
    [Arguments("ſ-Ä", "ſ-ä")]
    public Task Claim_Matches_Unicode_Capabilities(string required, string available) =>
        RunAsync(async (coordinator, _, _) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("Unicode") with
            {
                RequiredCapabilities = CapabilityRequirement.AnyOf(new Capability("other"), new Capability(required))
                    .And(CapabilityRequirement.AllOf(Capability.Docker)),
            };
            HashSet<Capability> capabilities = [new(available), Capability.Docker];
            await Assert.That(assignment.RequiredCapabilities.IsSatisfiedBy(capabilities)).IsTrue();
            await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var lease = await coordinator.DequeueModuleAsync(new WorkerId("unicode"), capabilities, timeout.Token);

            await Assert.That(lease).IsNotNull();
            await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(assignment.ModuleId);
            await Assert.That(lease.Assignment.RequiredCapabilities.Clauses.SelectMany(clause => clause).Select(capability => capability.Name))
                .Contains(required);
        });

    [Test]
    [Arguments("docker", "DOCKER")]
    [Arguments("gpu-ä", "GPU-Ä")]
    [Arguments("ς", "Σ")]
    [Arguments("𐐨", "𐐀")]
    public Task Claim_Migrates_Legacy_Queue_Classes(string required, string available) =>
        RunAsync(async (coordinator, client, keys) =>
        {
            var assignment = DistributedCoordinatorContract.CreateAssignment("Legacy") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability(required)),
            };
            var classJson = JsonSerializer.Serialize(new { RequiredCapabilities = new[] { new[] { required } }, AlwaysRun = false });
            var classId = RedisQueueMember.GetClassId(classJson);
            var member = RedisQueueMember.Create(classId, assignment, JsonSerializer.Serialize(assignment));
            await client.Scripts.ExecuteIntegerAsync(RespireScript.Create(RedisCoordinationScripts.Enqueue),
                [.. keys.ScriptKeys.Select(static key => (RespireKey) key)],
                [60_000, classId, classJson, member, assignment.ModuleId.Value]);

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var lease = await coordinator.DequeueModuleAsync(new WorkerId("upgraded"), new HashSet<Capability> { new(available) }, timeout.Token);

            await Assert.That(lease).IsNotNull();
            await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(assignment.ModuleId);
            await Assert.That(lease.Assignment.RequiredCapabilities.Clauses[0][0].Name).IsEqualTo(required);
            var leases = await client.Hashes.GetAsync<string>(keys.Leases, assignment.ModuleId.Value);
            using var storedLease = JsonDocument.Parse(leases!);
            await Assert.That(storedLease.RootElement.GetProperty("Member").GetString()).IsEqualTo(member);
            await Assert.That(await client.Hashes.CountAsync(keys.QueueClasses)).IsEqualTo(1L);
            await Assert.That(await client.Hashes.GetAsync<string>(keys.QueueIndex, assignment.ModuleId.Value)).IsNull();
        });

    [Test]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(6, false)]
    [Arguments(6, true)]
    public Task Capability_Migration_Preserves_Concurrent_Replacement_Or_Deletion(int keyIndex, bool deleted) =>
        RunAsync(async (_, client, keys) =>
        {
            var key = keys.ScriptKeys[keyIndex - 1];
            const string field = "concurrent";
            const string original = "{\"Capabilities\":[\"docker\"]}";
            const string replacement = "{\"Capabilities\":[\"gpu-ä\"]}";
            if (!deleted)
            {
                await client.Hashes.SetAsync(key, field, (RespireValue) replacement);
            }

            // The original snapshot was read before another worker replaced it or cleanup deleted it.
            await client.Scripts.ExecuteIntegerAsync(RespireScript.Create(RedisCoordinationScripts.NormalizeCapabilities),
                [.. keys.ScriptKeys.Select(static value => (RespireKey) value)],
                [keyIndex, field, original, "{\"NormalizedCapabilities\":[\"DOCKER\"]}"]);

            await Assert.That(await client.Hashes.GetAsync<string>(key, field)).IsEqualTo(deleted ? null : replacement);
        });

    [Test]
    [Arguments("ſ", "S")]
    [Arguments("ı", "I")]
    [Arguments("é", "e\u0301")]
    public Task Claim_Does_Not_Match_Distinct_Unicode_Capabilities(string required, string available) =>
        RunAsync(async (coordinator, _, _) =>
        {
            var unsupported = DistributedCoordinatorContract.CreateAssignment("Unsupported") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability(required)),
                Priority = ModulePriority.High,
            };
            var fallback = DistributedCoordinatorContract.CreateAssignment("Fallback");
            HashSet<Capability> capabilities = [new(available)];
            await Assert.That(unsupported.RequiredCapabilities.IsSatisfiedBy(capabilities)).IsFalse();
            await coordinator.EnqueueModuleAsync(unsupported, CancellationToken.None);
            await coordinator.EnqueueModuleAsync(fallback, CancellationToken.None);

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var lease = await coordinator.DequeueModuleAsync(new WorkerId("unicode"), capabilities, timeout.Token);

            await Assert.That(lease).IsNotNull();
            await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(fallback.ModuleId);
        });

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public Task Claim_Uses_Unicode_Capabilities_For_Fleet_Scarcity(bool legacyRegistrations) =>
        RunAsync(async (coordinator, client, keys, _) =>
        {
            var common = DistributedCoordinatorContract.CreateAssignment("Common") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("GPU-Ä")),
                CriticalPathWeight = TimeSpan.FromMinutes(10),
            };
            var scarce = DistributedCoordinatorContract.CreateAssignment("Scarce") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("rare")),
                CriticalPathWeight = TimeSpan.FromMinutes(1),
            };
            await coordinator.EnqueueModuleAsync(common, CancellationToken.None);
            await coordinator.EnqueueModuleAsync(scarce, CancellationToken.None);
            var worker = new WorkerRegistration
            {
                WorkerId = new WorkerId("unicode"),
                Capabilities = [new("gpu-ä"), new("rare")],
                RegisteredAt = DateTimeOffset.UtcNow,
            };
            await coordinator.RegisterWorkerAsync(worker, CancellationToken.None);
            var commonWorker = worker with
            {
                WorkerId = new WorkerId("common"),
                Capabilities = [new("gpu-ä")],
            };
            await coordinator.RegisterWorkerAsync(commonWorker, CancellationToken.None);
            if (legacyRegistrations)
            {
                foreach (var registration in new[] { worker, commonWorker })
                {
                    await client.Hashes.SetAsync(keys.Workers, registration.WorkerId.Value, (RespireValue) JsonSerializer.Serialize(registration));
                }
            }

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var lease = await coordinator.DequeueModuleAsync(worker.WorkerId, new HashSet<Capability> { new("GPU-Ä"), new("rare") }, timeout.Token);

            await Assert.That(lease).IsNotNull();
            await Assert.That(lease!.Assignment.ModuleId).IsEqualTo(scarce.ModuleId);
            var registrations = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
            await Assert.That(registrations.Single(registration => registration.WorkerId == worker.WorkerId).Capabilities[0].Name)
                .IsEqualTo("gpu-ä");
        }, 1, TimeSpan.FromSeconds(30));

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
                var lease = await coordinator.DequeueModuleAsync(new WorkerId("contract-worker"), new HashSet<Capability>(), CancellationToken.None);
                await Assert.That(await client.ExistsAsync(keys.Claims)).IsTrue();
                await coordinator.PublishResultAsync(
                    DistributedCoordinatorContract.CreateResult("Contract.Cleanup", "done"),
                    lease,
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
