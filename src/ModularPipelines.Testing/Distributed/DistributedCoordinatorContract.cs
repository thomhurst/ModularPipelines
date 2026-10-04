using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ModularPipelines.Distributed;

namespace ModularPipelines.Testing.Distributed;

/// <summary>
/// Behaviour every <see cref="IDistributedMasterCoordinator"/> implementation must provide.
/// Run each method against a fresh, isolated coordinator. Failed assertions throw
/// <see cref="InvalidOperationException"/> and require no particular test framework.
/// </summary>
public static class DistributedCoordinatorContract
{
    /// <summary>The lease timeout backends must be configured with for the lease tests.</summary>
    public static readonly TimeSpan LeaseTimeout = TimeSpan.FromMilliseconds(500);

    private static readonly WorkerId Worker = new("contract-worker");
    private static readonly WorkerId OtherWorker = new("contract-other");
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    /// <summary>Verifies that dequeuing returns an enqueued assignment with its metadata intact.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="waitUntilReady">Optional backend signal that the pending subscription is ready.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task EnqueueAndDequeueRoundTripsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var assignment = CreateAssignment("Contract.EnqueueDequeue") with
        {
            AlwaysRun = true,
            RequiredArtifacts = ["package"],
            SatisfiedConditionGroups = ["Contract.Group, Contract"],
        };
        var dequeueTask = coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady).ConfigureAwait(false);
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None).ConfigureAwait(false);
        var lease = await dequeueTask.WaitAsync(Timeout).ConfigureAwait(false);

        Check(lease is not null);
        Check(Equals(lease!.WorkerId, Worker));
        Check(!string.IsNullOrEmpty(lease.LeaseId));
        Check(Equals(lease.Assignment.ModuleId, assignment.ModuleId));
        Check(Equals(lease.Assignment.PipelineSchemaVersion, assignment.PipelineSchemaVersion));
        Check(lease.Assignment.AlwaysRun);
        Check(lease.Assignment.RequiredArtifacts.Order(StringComparer.Ordinal).SequenceEqual(assignment.RequiredArtifacts.Order(StringComparer.Ordinal)));
        Check(lease.Assignment.SatisfiedConditionGroups.Order(StringComparer.Ordinal).SequenceEqual(assignment.SatisfiedConditionGroups.Order(StringComparer.Ordinal)));
    }

    /// <summary>Verifies that result round trips after wait starts.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="waitUntilReady">Optional backend signal that the pending subscription is ready.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task ResultRoundTripsAfterWaitStartsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var result = CreateResult("Contract.ResultRoundTrip", "contract");
        var waitTask = coordinator.WaitForResultAsync(result.ModuleId, CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady).ConfigureAwait(false);
        await coordinator.PublishResultAsync(result, lease: null, CancellationToken.None).ConfigureAwait(false);
        var received = await waitTask.WaitAsync(Timeout).ConfigureAwait(false);

        Check(Equals(received.ModuleId, result.ModuleId));
        Check(Equals(received.WorkerId, result.WorkerId));
        Check(Equals(received.Payload, result.Payload));
    }

    /// <summary>Verifies that first published result is final.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task FirstPublishedResultIsFinalAsync(IDistributedMasterCoordinator coordinator)
    {
        var first = CreateResult("Contract.FirstResult", "first");
        var second = CreateResult("Contract.FirstResult", "second");

        await coordinator.PublishResultAsync(first, lease: null, CancellationToken.None).ConfigureAwait(false);
        await coordinator.PublishResultAsync(second, lease: null, CancellationToken.None).ConfigureAwait(false);
        var received = await coordinator.WaitForResultAsync(first.ModuleId, CancellationToken.None).WaitAsync(Timeout).ConfigureAwait(false);

        Check(Equals(received.Payload, first.Payload));
    }

    /// <summary>Verifies that completion unblocks pending dequeue.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="waitUntilReady">Optional backend signal that the pending subscription is ready.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task CompletionUnblocksPendingDequeueAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var dequeueTask = coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady).ConfigureAwait(false);
        await coordinator.SignalCompletionAsync(CancellationToken.None).ConfigureAwait(false);
        var result = await dequeueTask.WaitAsync(Timeout).ConfigureAwait(false);
        var later = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        Check(result is null);
        Check(later is null);
    }

    /// <summary>Verifies that canceled dequeue throws.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task CanceledDequeueThrowsAsync(IDistributedMasterCoordinator coordinator)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await ExpectThrowsAsync<OperationCanceledException>(() => coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                cancellation.Token)).ConfigureAwait(false);
    }

    /// <summary>Verifies that cancellation unblocks worker observer.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="waitUntilReady">Optional backend signal that the pending subscription is ready.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task CancellationUnblocksWorkerObserverAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var cancellationTask = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady).ConfigureAwait(false);
        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None).ConfigureAwait(false);
        var reason = await cancellationTask.WaitAsync(Timeout).ConfigureAwait(false);
        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None).ConfigureAwait(false);
        var laterReason = await coordinator.WaitForCancellationAsync(CancellationToken.None).WaitAsync(Timeout).ConfigureAwait(false);

        Check(Equals(reason, DistributedCancellationReason.PipelineFailed));
        Check(Equals(laterReason, DistributedCancellationReason.PipelineFailed));
    }

    /// <summary>Verifies that pipeline failure only releases always run work.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task PipelineFailureOnlyReleasesAlwaysRunWorkAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Normal"), CancellationToken.None).ConfigureAwait(false);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.AlwaysRun") with { AlwaysRun = true },
            CancellationToken.None).ConfigureAwait(false);

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None).ConfigureAwait(false);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);
        using var noMoreWork = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));

        Check(Equals(claimed!.Assignment.ModuleId, new ModuleId("Contract.AlwaysRun")));
        await ExpectThrowsAsync<OperationCanceledException>(() => coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                noMoreWork.Token)).ConfigureAwait(false);
    }

    /// <summary>Verifies that stop closes dequeue.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task StopClosesDequeueAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.StoppedAlwaysRun") with { AlwaysRun = true },
            CancellationToken.None).ConfigureAwait(false);

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None).ConfigureAwait(false);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        Check(claimed is null);
    }

    /// <summary>Verifies that withdraw removes queued assignment.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task WithdrawRemovesQueuedAssignmentAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Withdrawn"), CancellationToken.None).ConfigureAwait(false);
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Kept"), CancellationToken.None).ConfigureAwait(false);

        var withdrawn = await coordinator.WithdrawAssignmentAsync(new ModuleId("Contract.Withdrawn"), CancellationToken.None).ConfigureAwait(false);
        var withdrawnAgain = await coordinator.WithdrawAssignmentAsync(new ModuleId("Contract.Withdrawn"), CancellationToken.None).ConfigureAwait(false);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);
        using var noMoreWork = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));

        Check(withdrawn);
        Check(!(withdrawnAgain));
        Check(Equals(claimed!.Assignment.ModuleId, new ModuleId("Contract.Kept")));
        await ExpectThrowsAsync<OperationCanceledException>(() => coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                noMoreWork.Token)).ConfigureAwait(false);
    }

    /// <summary>Verifies that expired lease is requeued.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task ExpiredLeaseIsRequeuedAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Expired"), CancellationToken.None).ConfigureAwait(false);
        var firstClaim = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);
        var activeLeases = await coordinator.GetActiveLeasesAsync(CancellationToken.None).ConfigureAwait(false);

        await Task.Delay(LeaseTimeout * 3).ConfigureAwait(false);
        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None).ConfigureAwait(false);
        var secondClaim = await coordinator.DequeueModuleAsync(OtherWorker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        Check(firstClaim is not null);
        Check(activeLeases.Any(lease => lease.LeaseId == firstClaim.LeaseId));
        Check(requeued.Contains(new ModuleId("Contract.Expired")));
        Check(Equals(secondClaim!.WorkerId, OtherWorker));
        Check(!Equals(secondClaim.LeaseId, firstClaim.LeaseId));
    }

    /// <summary>Verifies that heartbeat renews lease.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task HeartbeatRenewsLeaseAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Renewed"), CancellationToken.None).ConfigureAwait(false);
        var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        for (var i = 0; i < 6; i++)
        {
            await Task.Delay(LeaseTimeout / 2).ConfigureAwait(false);
            await coordinator.SendHeartbeatAsync(
                new WorkerStatus { WorkerId = Worker, InFlightModules = [lease!.Assignment.ModuleId] },
                CancellationToken.None).ConfigureAwait(false);
        }

        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None).ConfigureAwait(false);

        Check(!requeued.Any());
        var activeLeases = await coordinator.GetActiveLeasesAsync(CancellationToken.None).ConfigureAwait(false);
        Check(activeLeases.Any(active => active.LeaseId == lease!.LeaseId));
    }

    /// <summary>Verifies that publishing releases lease.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task PublishingReleasesLeaseAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Released"), CancellationToken.None).ConfigureAwait(false);
        var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        await coordinator.PublishResultAsync(CreateResult("Contract.Released", "done"), lease, CancellationToken.None).ConfigureAwait(false);
        await Task.Delay(LeaseTimeout * 3).ConfigureAwait(false);
        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None).ConfigureAwait(false);

        Check(!(await coordinator.GetActiveLeasesAsync(CancellationToken.None).ConfigureAwait(false)).Any());
        Check(!requeued.Any());
    }

    /// <summary>Verifies that worker heartbeat keeps registration live.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task WorkerHeartbeatKeepsRegistrationLiveAsync(
        IDistributedMasterCoordinator coordinator)
    {
        var registration = CreateRegistration(Worker, [new Capability("dotnet")]);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None).ConfigureAwait(false);
        await coordinator.SendHeartbeatAsync(new WorkerStatus { WorkerId = Worker }, CancellationToken.None).ConfigureAwait(false);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None).ConfigureAwait(false);

        Check(workers.Any(worker => worker.WorkerId == Worker));
    }

    /// <summary>Verifies that duplicate live worker is rejected.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task DuplicateLiveWorkerIsRejectedAsync(IDistributedMasterCoordinator coordinator)
    {
        var registration = CreateRegistration(Worker, [new Capability("dotnet")]);
        var duplicate = registration with { RegisteredAt = registration.RegisteredAt.AddSeconds(1) };

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None).ConfigureAwait(false);

        // Registering the same session again is a reconnect.
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None).ConfigureAwait(false);
        await ExpectThrowsAsync<InvalidOperationException>(() => coordinator.RegisterWorkerAsync(duplicate, CancellationToken.None)).ConfigureAwait(false);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None).ConfigureAwait(false);
        Check(Equals(workers.Single(worker => worker.WorkerId == Worker).RegisteredAt, registration.RegisteredAt));
    }

    /// <summary>Verifies that claim prefers scarce capability work.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task ClaimPrefersScarceCapabilityWorkAsync(
        IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Common") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("common")),
                CriticalPathWeight = TimeSpan.FromMinutes(10),
            },
            CancellationToken.None).ConfigureAwait(false);

        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Linux") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("linux")),
                CriticalPathWeight = TimeSpan.FromMinutes(1),
            },
            CancellationToken.None).ConfigureAwait(false);

        // Register after enqueue to verify scarcity is evaluated against the fleet at claim time.
        await coordinator.RegisterWorkerAsync(
            CreateRegistration(new WorkerId("contract-1"), [new Capability("common"), new Capability("linux")]),
            CancellationToken.None).ConfigureAwait(false);
        await coordinator.RegisterWorkerAsync(
            CreateRegistration(new WorkerId("contract-2"), [new Capability("common")]),
            CancellationToken.None).ConfigureAwait(false);

        var claimed = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { new(new("common")), new(new("linux")) },
                CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        Check(Equals(claimed!.Assignment.ModuleId, new ModuleId("Contract.Linux")));
    }

    /// <summary>Verifies that claim prefers priority then critical path.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task ClaimPrefersPriorityThenCriticalPathAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Low") with { Priority = ModulePriority.Low }, CancellationToken.None).ConfigureAwait(false);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Short") with { CriticalPathWeight = TimeSpan.FromMinutes(1) },
            CancellationToken.None).ConfigureAwait(false);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Long") with { CriticalPathWeight = TimeSpan.FromMinutes(5) },
            CancellationToken.None).ConfigureAwait(false);

        var claims = new List<ModuleId>();
        for (var i = 0; i < 3; i++)
        {
            var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
                .WaitAsync(Timeout).ConfigureAwait(false);
            claims.Add(lease!.Assignment.ModuleId);
        }

        Check(claims.SequenceEqual(
                [new ModuleId("Contract.Long"), new ModuleId("Contract.Short"), new ModuleId("Contract.Low")]));
    }

    /// <summary>Verifies that claim matches alternative capabilities.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task ClaimMatchesAlternativeCapabilitiesAsync(
        IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Windows") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(Capability.Windows),
            },
            CancellationToken.None).ConfigureAwait(false);

        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Unix") with
            {
                RequiredCapabilities = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
                    .And(CapabilityRequirement.AllOf(Capability.Docker)),
            },
            CancellationToken.None).ConfigureAwait(false);

        var macClaim = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { Capability.MacOS, Capability.Docker },
                CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);
        var windowsClaim = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { Capability.Windows, Capability.Docker },
                CancellationToken.None)
            .WaitAsync(Timeout).ConfigureAwait(false);

        Check(Equals(macClaim!.Assignment.ModuleId, new ModuleId("Contract.Unix")));
        Check(Equals(macClaim.Assignment.RequiredCapabilities, CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
                .And(CapabilityRequirement.AllOf(Capability.Docker))));
        Check(Equals(windowsClaim!.Assignment.ModuleId, new ModuleId("Contract.Windows")));
    }

    /// <summary>Verifies that final metrics keep registration after heartbeat expires.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="heartbeatExpiration">A delay exceeding the configured worker heartbeat lifetime.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
        IDistributedMasterCoordinator coordinator,
        TimeSpan heartbeatExpiration)
    {
        var registration = CreateRegistration(Worker, [new Capability("dotnet")]);
        var finalStatus = new WorkerStatus
        {
            WorkerId = Worker,
            RunId = registration.RunId,
            IsFinal = true,
            UnattributedCommandCount = 2,
        };

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None).ConfigureAwait(false);
        await coordinator.SendHeartbeatAsync(finalStatus, CancellationToken.None).ConfigureAwait(false);
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None).ConfigureAwait(false);
        await Task.Delay(heartbeatExpiration).ConfigureAwait(false);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None).ConfigureAwait(false);
        var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None).ConfigureAwait(false);

        var retainedRegistration = workers.SingleOrDefault(worker => worker.WorkerId == Worker);
        Check(retainedRegistration is not null);
        var retainedStatus = statuses.Single(status => status.WorkerId == Worker);
        Check(retainedStatus.IsFinal);
        Check(Equals(retainedStatus.UnattributedCommandCount, 2));
    }

    /// <summary>Verifies that cancellation keeps concurrent observer subscribed.</summary>
    /// <param name="coordinator">A fresh coordinator dedicated to this check.</param>
    /// <param name="waitUntilReady">Optional backend signal that the pending subscription is ready.</param>
    /// <returns>The asynchronous contract check.</returns>
    public static async Task CancellationKeepsConcurrentObserverSubscribedAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        using var firstCancellation = new CancellationTokenSource();
        var firstObserver = coordinator.WaitForCancellationAsync(firstCancellation.Token);
        var remainingObserver = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady).ConfigureAwait(false);
        firstCancellation.Cancel();
        await ExpectThrowsAsync<OperationCanceledException>(() => firstObserver).ConfigureAwait(false);

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None).ConfigureAwait(false);
        Check(Equals(await remainingObserver.WaitAsync(Timeout).ConfigureAwait(false), DistributedCancellationReason.Stopped));
    }

    /// <summary>Creates a sample assignment for coordinator tests.</summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <returns>Contract test data.</returns>
    public static ModuleAssignment CreateAssignment(string moduleId) => new()
    {
        ModuleId = new ModuleId(moduleId),
        RequiredCapabilities = CapabilityRequirement.None,
        PipelineSchemaVersion = "contract-schema",
    };

    /// <summary>Creates a sample serialized result for coordinator tests.</summary>
    /// <param name="moduleId">The module identifier.</param>
    /// <param name="value">The string value for the result payload.</param>
    /// <returns>Contract test data.</returns>
    public static SerializedModuleResult CreateResult(string moduleId, string value) => new()
    {
        ModuleId = new ModuleId(moduleId),
        WorkerId = Worker,
        Payload = $"{{\"value\":{JsonSerializer.Serialize(value)}}}",
        CompletedAt = DateTimeOffset.UtcNow,
    };

    private static WorkerRegistration CreateRegistration(WorkerId workerId, IReadOnlyList<Capability> capabilities) => new()
    {
        WorkerId = workerId,
        Capabilities = capabilities,
        RegisteredAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero),
        RunId = "contract-run",
    };

    private static async Task WaitUntilReadyAsync(Task? waitUntilReady)
    {
        if (waitUntilReady is not null)
        {
            await waitUntilReady.WaitAsync(Timeout).ConfigureAwait(false);
        }
    }

    private static void Check([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression(nameof(condition))] string? expression = null)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"Coordinator contract failed: {expression}");
        }
    }

    private static async Task ExpectThrowsAsync<TException>(Func<Task> action)
        where TException : Exception
    {
        try
        {
            await action().WaitAsync(Timeout).ConfigureAwait(false);
        }
        catch (TException)
        {
            return;
        }
        catch (TimeoutException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Coordinator contract expected {typeof(TException).Name} but got {exception.GetType().Name}.", exception);
        }

        throw new InvalidOperationException($"Coordinator contract expected {typeof(TException).Name}.");
    }
}
