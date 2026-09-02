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
            Capabilities: new HashSet<Capability> { "linux" },
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
    public async Task Cancellation_Unblocks_Worker_Observer()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.CancellationUnblocksWorkerObserverAsync(coordinator);
    }

    [Test]
    public async Task Heartbeat_Keeps_Worker_Registration_Live()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.WorkerHeartbeatKeepsRegistrationLiveAsync(coordinator);
    }

    [Test]
    public async Task Stale_Worker_Is_Excluded_From_Live_Registrations()
    {
        var coordinator = new InMemoryDistributedCoordinator(
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                WorkerTimeout = TimeSpan.FromMilliseconds(10),
            }));
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(1, new HashSet<Capability>(), DateTimeOffset.UtcNow),
            CancellationToken.None);

        await Task.Delay(30);
        var workers = await coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers).IsEmpty();
    }

    [Test]
    public async Task Dequeue_With_Capability_Filtering()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var dockerAssignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability> { "docker" },
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, 0, false));

        await coordinator.EnqueueModuleAsync(dockerAssignment, CancellationToken.None);

        // Worker without docker capability should not get the assignment
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }
}
