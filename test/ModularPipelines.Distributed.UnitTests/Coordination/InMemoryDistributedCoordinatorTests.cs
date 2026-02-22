using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class InMemoryDistributedCoordinatorTests
{
    [Test]
    public async Task Enqueue_And_Dequeue_Returns_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        var result = await coordinator.DequeueModuleAsync(
            new HashSet<string>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test.Module");
    }

    [Test]
    public async Task Publish_And_Wait_For_Result()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var serializedResult = new SerializedModuleResult(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            WorkerIndex: 1,
            SerializedJson: "{}",
            CompletedAt: DateTimeOffset.UtcNow);

        // Start waiting before publishing
        var waitTask = coordinator.WaitForResultAsync("Test.Module", CancellationToken.None);

        // Publish after a small delay
        await Task.Delay(50);
        await coordinator.PublishResultAsync(serializedResult, CancellationToken.None);

        var result = await waitTask;

        await Assert.That(result.ModuleTypeName).IsEqualTo("Test.Module");
        await Assert.That(result.WorkerIndex).IsEqualTo(1);
    }

    [Test]
    public async Task RegisterWorker_And_GetRegisteredWorkers()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var registration = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Connected,
            CurrentModule: null);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);

        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Count).IsEqualTo(1);
        await Assert.That(workers[0].WorkerIndex).IsEqualTo(1);
    }

    [Test]
    public async Task BroadcastCancellation_And_Check()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        // No cancellation initially
        var signal = await coordinator.IsCancellationRequestedAsync(CancellationToken.None);
        await Assert.That(signal).IsNull();

        // Broadcast cancellation
        await coordinator.BroadcastCancellationAsync("Test reason", CancellationToken.None);

        signal = await coordinator.IsCancellationRequestedAsync(CancellationToken.None);
        await Assert.That(signal).IsNotNull();
        await Assert.That(signal!.Reason).IsEqualTo("Test reason");
    }

    [Test]
    public async Task Dequeue_With_Capability_Filtering()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var dockerAssignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        await coordinator.EnqueueModuleAsync(dockerAssignment, CancellationToken.None);

        // Worker without docker capability should not get the assignment
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }
}
