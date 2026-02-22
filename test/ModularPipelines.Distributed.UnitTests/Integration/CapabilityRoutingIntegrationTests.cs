using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CapabilityRoutingIntegrationTests
{
    [Test]
    public async Task Capable_Worker_Receives_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker with docker capability
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux", "docker" }, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Docker.Module");
    }

    [Test]
    public async Task Incapable_Worker_Does_Not_Receive_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker without docker capability - should timeout
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task CapabilityMatcher_Validates_Worker_Assignments()
    {
        var dockerWorker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "linux", "docker" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var plainWorker = new WorkerRegistration(
            WorkerIndex: 2,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var dockerAssignment = new ModuleAssignment(
            ModuleTypeName: "Docker.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        var plainAssignment = new ModuleAssignment(
            ModuleTypeName: "Plain.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        // Docker worker can execute both
        await Assert.That(CapabilityMatcher.CanExecute(dockerAssignment, dockerWorker)).IsTrue();
        await Assert.That(CapabilityMatcher.CanExecute(plainAssignment, dockerWorker)).IsTrue();

        // Plain worker can only execute plain assignment
        await Assert.That(CapabilityMatcher.CanExecute(dockerAssignment, plainWorker)).IsFalse();
        await Assert.That(CapabilityMatcher.CanExecute(plainAssignment, plainWorker)).IsTrue();
    }
}
