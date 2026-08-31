using ModularPipelines.Distributed.Coordination;
using ModularPipelines.TestHelpers.Distributed;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class InMemoryDistributedCoordinatorTests
{
    [Test]
    public async Task Enqueue_And_Dequeue_Returns_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync(coordinator);
    }

    [Test]
    public async Task Publish_And_Wait_For_Result()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.ResultRoundTripsAfterWaitStartsAsync(coordinator);
    }

    [Test]
    public async Task RegisterWorker_And_GetRegisteredWorkers()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var registration = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow);

        await coordinator.RegisterWorkerAsync(registration, CancellationToken.None);

        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Count).IsEqualTo(1);
        await Assert.That(workers[0].WorkerIndex).IsEqualTo(1);
    }

    [Test]
    public async Task SignalCompletion_CausesDequeueToReturnNull()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.CompletionUnblocksPendingDequeueAsync(coordinator);
    }

    [Test]
    public async Task Dequeue_With_Capability_Filtering()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var dockerAssignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, 0, false));

        await coordinator.EnqueueModuleAsync(dockerAssignment, CancellationToken.None);

        // Worker without docker capability should not get the assignment
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }
}
