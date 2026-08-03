using ModularPipelines.Distributed;

namespace ModularPipelines.TestHelpers.Distributed;

public static class DistributedCoordinatorContract
{
    public static async Task EnqueueAndDequeueRoundTripsAsync(IDistributedCoordinator coordinator)
    {
        var assignment = CreateAssignment("Contract.EnqueueDequeue");
        var dequeueTask = coordinator.DequeueModuleAsync(new HashSet<string>(), CancellationToken.None);

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
        var result = await dequeueTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo(assignment.ModuleTypeName);
        await Assert.That(result.ResultTypeName).IsEqualTo(assignment.ResultTypeName);
        await Assert.That(result.Configuration).IsEqualTo(assignment.Configuration);
    }

    public static async Task ResultRoundTripsAfterWaitStartsAsync(IDistributedCoordinator coordinator)
    {
        var result = CreateResult("Contract.ResultRoundTrip");
        var waitTask = coordinator.WaitForResultAsync(result.ModuleTypeName, CancellationToken.None);

        await coordinator.PublishResultAsync(result, CancellationToken.None);
        var received = await waitTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(received).IsEqualTo(result);
    }

    public static async Task CompletionUnblocksPendingDequeueAsync(IDistributedCoordinator coordinator)
    {
        var dequeueTask = coordinator.DequeueModuleAsync(new HashSet<string>(), CancellationToken.None);

        await coordinator.SignalCompletionAsync(CancellationToken.None);
        var result = await dequeueTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(result).IsNull();
    }

    private static ModuleAssignment CreateAssignment(string moduleTypeName)
    {
        return new ModuleAssignment(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));
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
