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
    public async Task Claim_Prefers_Scarce_Capability_Work()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await DistributedCoordinatorContract.ClaimPrefersScarceCapabilityWorkAsync(coordinator);
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
    public async Task Final_Metrics_Keep_Worker_Registration_After_Heartbeat_Expires()
    {
        var coordinator = new InMemoryDistributedCoordinator(
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                WorkerTimeout = TimeSpan.FromMilliseconds(10),
            }));

        await DistributedCoordinatorContract.FinalMetricsKeepRegistrationAfterHeartbeatExpiresAsync(
            coordinator,
            TimeSpan.FromMilliseconds(30));
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
            Configuration: new ModuleAssignmentConfiguration(null, false));

        await coordinator.EnqueueModuleAsync(dockerAssignment, CancellationToken.None);

        // Worker without docker capability should not get the assignment
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task Dequeue_Prefers_Higher_User_Priority()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Low", ModulePriority.Low),
            CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Critical", ModulePriority.Critical),
            CancellationToken.None);

        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability>(),
            CancellationToken.None);

        await Assert.That(result!.ModuleTypeName).IsEqualTo("Critical");
    }

    [Test]
    public async Task Dequeue_Prefers_Longer_Critical_Path_At_Equal_Priority()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Short", criticalPathWeight: TimeSpan.FromMinutes(1)),
            CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment("Long", criticalPathWeight: TimeSpan.FromMinutes(10)),
            CancellationToken.None);

        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability>(),
            CancellationToken.None);

        await Assert.That(result!.ModuleTypeName).IsEqualTo("Long");
    }

    [Test]
    public async Task Dequeue_Prefers_Work_Eligible_On_Fewer_Workers()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(1, new HashSet<Capability> { Capability.Linux }, DateTimeOffset.UtcNow),
            CancellationToken.None);
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(2, new HashSet<Capability> { Capability.Windows }, DateTimeOffset.UtcNow),
            CancellationToken.None);
        await coordinator.EnqueueModuleAsync(CreateAssignment("Generic"), CancellationToken.None);
        await coordinator.EnqueueModuleAsync(
            CreateAssignment(
                "LinuxOnly",
                requiredCapabilities: new HashSet<Capability> { Capability.Linux }),
            CancellationToken.None);

        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { Capability.Linux },
            CancellationToken.None);

        await Assert.That(result!.ModuleTypeName).IsEqualTo("LinuxOnly");
    }

    private static ModuleAssignment CreateAssignment(
        string moduleTypeName,
        ModulePriority priority = ModulePriority.Normal,
        TimeSpan criticalPathWeight = default,
        IReadOnlySet<Capability>? requiredCapabilities = null)
    {
        return new ModuleAssignment(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            RequiredCapabilities: requiredCapabilities ?? new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false))
        {
            Priority = priority,
            CriticalPathWeight = criticalPathWeight,
        };
    }
}
