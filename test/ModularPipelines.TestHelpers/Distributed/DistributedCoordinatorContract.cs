using ModularPipelines.Distributed;

namespace ModularPipelines.TestHelpers.Distributed;

/// <summary>
/// Behaviour every <see cref="IDistributedMasterCoordinator"/> implementation must provide.
/// Each backend's tests run these methods against a fresh coordinator.
/// </summary>
public static class DistributedCoordinatorContract
{
    /// <summary>The lease timeout backends must be configured with for the lease tests.</summary>
    public static readonly TimeSpan LeaseTimeout = TimeSpan.FromMilliseconds(500);

    private static readonly WorkerId Worker = new("contract-worker");
    private static readonly WorkerId OtherWorker = new("contract-other");
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

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

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
        var lease = await dequeueTask.WaitAsync(Timeout);

        await Assert.That(lease).IsNotNull();
        await Assert.That(lease!.WorkerId).IsEqualTo(Worker);
        await Assert.That(lease.LeaseId).IsNotEmpty();
        await Assert.That(lease.Assignment.ModuleId).IsEqualTo(assignment.ModuleId);
        await Assert.That(lease.Assignment.PipelineSchemaVersion).IsEqualTo(assignment.PipelineSchemaVersion);
        await Assert.That(lease.Assignment.AlwaysRun).IsTrue();
        await Assert.That(lease.Assignment.RequiredArtifacts).IsEquivalentTo(assignment.RequiredArtifacts);
        await Assert.That(lease.Assignment.SatisfiedConditionGroups).IsEquivalentTo(assignment.SatisfiedConditionGroups);
    }

    public static async Task ResultRoundTripsAfterWaitStartsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var result = CreateResult("Contract.ResultRoundTrip", "contract");
        var waitTask = coordinator.WaitForResultAsync(result.ModuleId, CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.PublishResultAsync(result, lease: null, CancellationToken.None);
        var received = await waitTask.WaitAsync(Timeout);

        await Assert.That(received.ModuleId).IsEqualTo(result.ModuleId);
        await Assert.That(received.WorkerId).IsEqualTo(result.WorkerId);
        await Assert.That(received.Payload).IsEqualTo(result.Payload);
    }

    public static async Task FirstPublishedResultIsFinalAsync(IDistributedMasterCoordinator coordinator)
    {
        var first = CreateResult("Contract.FirstResult", "first");
        var second = CreateResult("Contract.FirstResult", "second");

        await coordinator.PublishResultAsync(first, lease: null, CancellationToken.None);
        await coordinator.PublishResultAsync(second, lease: null, CancellationToken.None);
        var received = await coordinator.WaitForResultAsync(first.ModuleId, CancellationToken.None).WaitAsync(Timeout);

        await Assert.That(received.Payload).IsEqualTo(first.Payload);
    }

    public static async Task CompletionUnblocksPendingDequeueAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var dequeueTask = coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.SignalCompletionAsync(CancellationToken.None);
        var result = await dequeueTask.WaitAsync(Timeout);
        var later = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(result).IsNull();
        await Assert.That(later).IsNull();
    }

    public static async Task CanceledDequeueThrowsAsync(IDistributedMasterCoordinator coordinator)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    public static async Task CancellationUnblocksWorkerObserverAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var cancellationTask = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None);
        var reason = await cancellationTask.WaitAsync(Timeout);
        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None);
        var laterReason = await coordinator.WaitForCancellationAsync(CancellationToken.None).WaitAsync(Timeout);

        await Assert.That(reason).IsEqualTo(DistributedCancellationReason.PipelineFailed);
        await Assert.That(laterReason).IsEqualTo(DistributedCancellationReason.PipelineFailed);
    }

    public static async Task PipelineFailureOnlyReleasesAlwaysRunWorkAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Normal"), CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.AlwaysRun") with { AlwaysRun = true },
            CancellationToken.None);

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);
        using var noMoreWork = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));

        await Assert.That(claimed!.Assignment.ModuleId).IsEqualTo(new ModuleId("Contract.AlwaysRun"));
        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                noMoreWork.Token))
            .Throws<OperationCanceledException>();
    }

    public static async Task StopClosesDequeueAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.StoppedAlwaysRun") with { AlwaysRun = true },
            CancellationToken.None);

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(claimed).IsNull();
    }

    public static async Task WithdrawRemovesQueuedAssignmentAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Withdrawn"), CancellationToken.None);
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Kept"), CancellationToken.None);

        var withdrawn = await coordinator.WithdrawAssignmentAsync(new ModuleId("Contract.Withdrawn"), CancellationToken.None);
        var withdrawnAgain = await coordinator.WithdrawAssignmentAsync(new ModuleId("Contract.Withdrawn"), CancellationToken.None);
        var claimed = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);
        using var noMoreWork = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));

        await Assert.That(withdrawn).IsTrue();
        await Assert.That(withdrawnAgain).IsFalse();
        await Assert.That(claimed!.Assignment.ModuleId).IsEqualTo(new ModuleId("Contract.Kept"));
        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability>(),
                noMoreWork.Token))
            .Throws<OperationCanceledException>();
    }

    public static async Task ExpiredLeaseIsRequeuedAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Expired"), CancellationToken.None);
        var firstClaim = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);
        var activeLeases = await coordinator.GetActiveLeasesAsync(CancellationToken.None);

        await Task.Delay(LeaseTimeout * 3);
        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);
        var secondClaim = await coordinator.DequeueModuleAsync(OtherWorker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(activeLeases.Select(lease => lease.LeaseId)).Contains(firstClaim!.LeaseId);
        await Assert.That(requeued).Contains(new ModuleId("Contract.Expired"));
        await Assert.That(secondClaim!.WorkerId).IsEqualTo(OtherWorker);
        await Assert.That(secondClaim.LeaseId).IsNotEqualTo(firstClaim.LeaseId);
    }

    public static async Task HeartbeatRenewsLeaseAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Renewed"), CancellationToken.None);
        var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        for (var i = 0; i < 6; i++)
        {
            await Task.Delay(LeaseTimeout / 2);
            await coordinator.SendHeartbeatAsync(
                new WorkerStatus { WorkerId = Worker, InFlightModules = [lease!.Assignment.ModuleId] },
                CancellationToken.None);
        }

        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);

        await Assert.That(requeued).IsEmpty();
        await Assert.That((await coordinator.GetActiveLeasesAsync(CancellationToken.None)).Select(active => active.LeaseId))
            .Contains(lease!.LeaseId);
    }

    public static async Task PublishingReleasesLeaseAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Released"), CancellationToken.None);
        var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
            .WaitAsync(Timeout);

        await coordinator.PublishResultAsync(CreateResult("Contract.Released", "done"), lease, CancellationToken.None);
        await Task.Delay(LeaseTimeout * 3);
        var requeued = await coordinator.RequeueExpiredLeasesAsync(CancellationToken.None);

        await Assert.That(await coordinator.GetActiveLeasesAsync(CancellationToken.None)).IsEmpty();
        await Assert.That(requeued).IsEmpty();
    }

    public static async Task WorkerHeartbeatKeepsRegistrationLiveAsync(
        IDistributedMasterCoordinator coordinator)
    {
        var registration = CreateRegistration(Worker, [new Capability("dotnet")]);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await coordinator.SendHeartbeatAsync(new WorkerStatus { WorkerId = Worker }, CancellationToken.None);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Select(worker => worker.WorkerId)).Contains(Worker);
    }

    public static async Task DuplicateLiveWorkerIsRejectedAsync(IDistributedMasterCoordinator coordinator)
    {
        var registration = CreateRegistration(Worker, [new Capability("dotnet")]);
        var duplicate = registration with { RegisteredAt = registration.RegisteredAt.AddSeconds(1) };

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);

        // Registering the same session again is a reconnect.
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await Assert.That(async () => await coordinator.RegisterWorkerAsync(duplicate, CancellationToken.None))
            .Throws<InvalidOperationException>();
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
        await Assert.That(workers.Single(worker => worker.WorkerId == Worker).RegisteredAt)
            .IsEqualTo(registration.RegisteredAt);
    }

    public static async Task ClaimPrefersScarceCapabilityWorkAsync(
        IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Common") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("common")),
                CriticalPathWeight = TimeSpan.FromMinutes(10),
            },
            CancellationToken.None);

        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Linux") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("linux")),
                CriticalPathWeight = TimeSpan.FromMinutes(1),
            },
            CancellationToken.None);

        // Register after enqueue to verify scarcity is evaluated against the fleet at claim time.
        await coordinator.RegisterWorkerAsync(
            CreateRegistration(new WorkerId("contract-1"), [new Capability("common"), new Capability("linux")]),
            CancellationToken.None);
        await coordinator.RegisterWorkerAsync(
            CreateRegistration(new WorkerId("contract-2"), [new Capability("common")]),
            CancellationToken.None);

        var claimed = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { new(new("common")), new(new("linux")) },
                CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(claimed!.Assignment.ModuleId).IsEqualTo(new ModuleId("Contract.Linux"));
    }

    public static async Task ClaimPrefersPriorityThenCriticalPathAsync(IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(CreateAssignment("Contract.Low") with { Priority = ModulePriority.Low }, CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Short") with { CriticalPathWeight = TimeSpan.FromMinutes(1) },
            CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Long") with { CriticalPathWeight = TimeSpan.FromMinutes(5) },
            CancellationToken.None);

        var claims = new List<ModuleId>();
        for (var i = 0; i < 3; i++)
        {
            var lease = await coordinator.DequeueModuleAsync(Worker, new HashSet<Capability>(), CancellationToken.None)
                .WaitAsync(Timeout);
            claims.Add(lease!.Assignment.ModuleId);
        }

        await Assert.That(claims.SequenceEqual(
                [new ModuleId("Contract.Long"), new ModuleId("Contract.Short"), new ModuleId("Contract.Low")]))
            .IsTrue();
    }

    public static async Task ClaimMatchesAlternativeCapabilitiesAsync(
        IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Windows") with
            {
                RequiredCapabilities = CapabilityRequirement.AllOf(Capability.Windows),
            },
            CancellationToken.None);

        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Unix") with
            {
                RequiredCapabilities = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
                    .And(CapabilityRequirement.AllOf(Capability.Docker)),
            },
            CancellationToken.None);

        var macClaim = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { Capability.MacOS, Capability.Docker },
                CancellationToken.None)
            .WaitAsync(Timeout);
        var windowsClaim = await coordinator.DequeueModuleAsync(
                Worker,
                new HashSet<Capability> { Capability.Windows, Capability.Docker },
                CancellationToken.None)
            .WaitAsync(Timeout);

        await Assert.That(macClaim!.Assignment.ModuleId).IsEqualTo(new ModuleId("Contract.Unix"));
        await Assert.That(macClaim.Assignment.RequiredCapabilities).IsEqualTo(
            CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
                .And(CapabilityRequirement.AllOf(Capability.Docker)));
        await Assert.That(windowsClaim!.Assignment.ModuleId).IsEqualTo(new ModuleId("Contract.Windows"));
    }

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

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await coordinator.SendHeartbeatAsync(finalStatus, CancellationToken.None);
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await Task.Delay(heartbeatExpiration);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
        var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None);

        var retainedRegistration = workers.SingleOrDefault(worker => worker.WorkerId == Worker);
        await Assert.That(retainedRegistration).IsNotNull();
        var retainedStatus = statuses.Single(status => status.WorkerId == Worker);
        await Assert.That(retainedStatus.IsFinal).IsTrue();
        await Assert.That(retainedStatus.UnattributedCommandCount).IsEqualTo(2);
    }

    public static async Task CancellationKeepsConcurrentObserverSubscribedAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        using var firstCancellation = new CancellationTokenSource();
        var firstObserver = coordinator.WaitForCancellationAsync(firstCancellation.Token);
        var remainingObserver = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        firstCancellation.Cancel();
        await Assert.That(async () => await firstObserver).Throws<OperationCanceledException>();

        await coordinator.BroadcastCancellationAsync(DistributedCancellationReason.Stopped, CancellationToken.None);
        await Assert.That(await remainingObserver.WaitAsync(Timeout)).IsEqualTo(DistributedCancellationReason.Stopped);
    }

    public static ModuleAssignment CreateAssignment(string moduleId) => new()
    {
        ModuleId = new ModuleId(moduleId),
        RequiredCapabilities = CapabilityRequirement.None,
        PipelineSchemaVersion = "contract-schema",
    };

    public static SerializedModuleResult CreateResult(string moduleId, string value) => new()
    {
        ModuleId = new ModuleId(moduleId),
        WorkerId = Worker,
        Payload = $"{{\"value\":\"{value}\"}}",
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
            await waitUntilReady.WaitAsync(Timeout);
        }
    }
}
