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
        await Assert.That(result!.ModuleTypeName).IsEqualTo(assignment.ModuleTypeName);
        await Assert.That(result.ResultTypeName).IsEqualTo(assignment.ResultTypeName);
        await Assert.That(result.Configuration).IsEqualTo(assignment.Configuration);
    }

    public static async Task ResultRoundTripsAfterWaitStartsAsync(
        IDistributedMasterCoordinator coordinator,
        Task? waitUntilReady = null)
    {
        var result = CreateResult("Contract.ResultRoundTrip");
        var waitTask = coordinator.WaitForResultAsync(result.ModuleTypeName, CancellationToken.None);

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
            new HashSet<Capability> { new("dotnet") },
            DateTimeOffset.UtcNow);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await coordinator.SendHeartbeatAsync(registration.WorkerIndex, CancellationToken.None);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Select(worker => worker.WorkerIndex))
            .Contains(registration.WorkerIndex);
    }

    public static async Task FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
        IDistributedMasterCoordinator coordinator,
        TimeSpan heartbeatExpiration)
    {
        var registration = new WorkerRegistration(
            1,
            new HashSet<Capability> { new("dotnet") },
            DateTimeOffset.UtcNow)
        {
            UnattributedCommandCount = 0,
        };

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        await Task.Delay(heartbeatExpiration);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        var retainedRegistration = workers.SingleOrDefault(worker =>
            worker.WorkerIndex == registration.WorkerIndex);
        await Assert.That(retainedRegistration).IsNotNull();
        await Assert.That(retainedRegistration!.UnattributedCommandCount)
            .IsEqualTo(registration.UnattributedCommandCount);
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

    private static ModuleAssignment CreateAssignment(string moduleTypeName)
    {
        return new ModuleAssignment(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, 0, false));
    }

    private static SerializedModuleResult CreateResult(string moduleTypeName)
    {
        return new SerializedModuleResult(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            WorkerIndex: 1,
            SerializedJson: "{\"value\":\"contract\"}",
            CompletedAt: DateTimeOffset.UtcNow);
    }
}
