using ModularPipelines.Distributed;

namespace ModularPipelines.TestHelpers.Distributed;

public static class DistributedCoordinatorContract
{
    public static async Task EnqueueAndDequeueRoundTripsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var assignment = CreateAssignment("Contract.EnqueueDequeue");
        var dequeueTask = coordinator.DequeueModuleAsync(new HashSet<Capability>(), CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
        var result = await dequeueTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleId).IsEqualTo(assignment.ModuleId);
        await Assert.That(result.PipelineSchemaVersion).IsEqualTo(assignment.PipelineSchemaVersion);
        await Assert.That(result.Configuration).IsEqualTo(assignment.Configuration);
    }

    public static async Task ResultRoundTripsAfterWaitStartsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var result = CreateResult("Contract.ResultRoundTrip");
        var waitTask = coordinator.WaitForResultAsync(result.ModuleId, CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.PublishResultAsync(result, CancellationToken.None);
        var received = await waitTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(received).IsEqualTo(result);
    }

    public static async Task CompletionUnblocksPendingDequeueAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var dequeueTask = coordinator.DequeueModuleAsync(new HashSet<Capability>(), CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.SignalCompletionAsync(CancellationToken.None);
        var result = await dequeueTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(result).IsNull();
    }

    public static async Task CancellationUnblocksWorkerObserverAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var cancellationTask = coordinator.WaitForCancellationAsync(CancellationToken.None);

        await WaitUntilReadyAsync(waitUntilReady);
        await coordinator.BroadcastCancellationAsync(CancellationToken.None);
        await cancellationTask.WaitAsync(TimeSpan.FromSeconds(5));
    }

    public static async Task WorkerHeartbeatKeepsRegistrationLiveAsync(
        IDistributedMasterCoordinator coordinator)
    {
        var registration = new WorkerRegistration(
            1,
            [new Capability("dotnet")],
            DateTimeOffset.UtcNow);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await coordinator.SendHeartbeatAsync(
            new WorkerStatus(registration.WorkerIndex),
            CancellationToken.None);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Select(worker => worker.WorkerIndex))
            .Contains(registration.WorkerIndex);
    }

    public static async Task ClaimPrefersScarceCapabilityWorkAsync(
        IDistributedMasterCoordinator coordinator)
    {
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Common") with
            {
                RequiredCapabilities = ["common"],
                CriticalPathWeight = TimeSpan.FromMinutes(10),
            },
            CancellationToken.None);

        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Contract.Linux") with
            {
                RequiredCapabilities = ["linux"],
                CriticalPathWeight = TimeSpan.FromMinutes(1),
            },
            CancellationToken.None);

        // Register after enqueue to verify scarcity is evaluated against the fleet at claim time.
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(1, ["common", "linux"], DateTimeOffset.UtcNow),
            CancellationToken.None);
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(2, ["common"], DateTimeOffset.UtcNow),
            CancellationToken.None);

        var claimed = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { "common", "linux" },
            CancellationToken.None);

        await Assert.That(claimed!.ModuleId).IsEqualTo("Contract.Linux");
    }

    public static async Task FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
        IDistributedMasterCoordinator coordinator,
        TimeSpan heartbeatExpiration)
    {
        var registration = new WorkerRegistration(
            1,
            [new Capability("dotnet")],
            DateTimeOffset.UtcNow);
        var finalStatus = new WorkerStatus(registration.WorkerIndex)
        {
            UnattributedCommandCount = 0,
        };

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await coordinator.SendHeartbeatAsync(finalStatus, CancellationToken.None);
        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await Task.Delay(heartbeatExpiration);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);
        var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None);

        var retainedRegistration = workers.SingleOrDefault(worker =>
            worker.WorkerIndex == registration.WorkerIndex);
        await Assert.That(retainedRegistration).IsNotNull();
        await Assert.That(statuses.Single(status => status.WorkerIndex == registration.WorkerIndex))
            .IsEqualTo(finalStatus);
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

        await coordinator.BroadcastCancellationAsync(CancellationToken.None);
        await remainingObserver.WaitAsync(TimeSpan.FromSeconds(5));
    }

    private static async Task WaitUntilReadyAsync(Task? waitUntilReady)
    {
        if (waitUntilReady is not null)
        {
            await waitUntilReady.WaitAsync(TimeSpan.FromSeconds(5));
        }
    }

    private static ModuleAssignment CreateAssignment(ModuleId moduleId)
    {
        return new ModuleAssignment(
            ModuleId: new ModuleId(moduleId),
            RequiredCapabilities: [],
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));
    }

    private static SerializedModuleResult CreateResult(ModuleId moduleId)
    {
        return new SerializedModuleResult(
            ModuleId: new ModuleId(moduleId),
            WorkerIndex: 1,
            Payload: "{\"value\":\"contract\"}",
            CompletedAt: DateTimeOffset.UtcNow);
    }
}
