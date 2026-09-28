using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CapabilityRoutingIntegrationTests
{
    [Test]
    public async Task Capable_Worker_Receives_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment
        {
            ModuleId = "Docker.Module",
            RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("docker")),
            AlwaysRun = false,
            PipelineSchemaVersion = string.Empty,
        };

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker with docker capability
        var result = await coordinator.DequeueModuleAsync(
            DistributedTestData.Worker,
            new HashSet<Capability> { new("linux"), new("docker") },
            CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Assignment.ModuleId).IsEqualTo("Docker.Module");
    }

    [Test]
    public async Task Incapable_Worker_Does_Not_Receive_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment
        {
            ModuleId = "Docker.Module",
            RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("docker")),
            AlwaysRun = false,
            PipelineSchemaVersion = string.Empty,
        };

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker without docker capability - should timeout
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                DistributedTestData.Worker,
                new HashSet<Capability> { new("linux") },
                cts.Token))
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task Requirement_Validates_Worker_Assignments()
    {
        var dockerWorker = new WorkerRegistration
        {
            WorkerId = WorkerId.FromInstanceIndex(1),
            Capabilities = [new Capability("linux"), new Capability("docker")],
            RegisteredAt = DateTimeOffset.UtcNow,
        };

        var plainWorker = new WorkerRegistration
        {
            WorkerId = WorkerId.FromInstanceIndex(2),
            Capabilities = [new Capability("linux")],
            RegisteredAt = DateTimeOffset.UtcNow,
        };

        var dockerAssignment = new ModuleAssignment
        {
            ModuleId = "Docker.Module",
            RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("docker")),
            AlwaysRun = false,
            PipelineSchemaVersion = string.Empty,
        };

        var plainAssignment = new ModuleAssignment
        {
            ModuleId = "Plain.Module",
            RequiredCapabilities = CapabilityRequirement.None,
            AlwaysRun = false,
            PipelineSchemaVersion = string.Empty,
        };

        // Docker worker can execute both
        await Assert.That(dockerAssignment.RequiredCapabilities.IsSatisfiedBy(dockerWorker.Capabilities)).IsTrue();
        await Assert.That(plainAssignment.RequiredCapabilities.IsSatisfiedBy(dockerWorker.Capabilities)).IsTrue();

        // Plain worker can only execute plain assignment
        await Assert.That(dockerAssignment.RequiredCapabilities.IsSatisfiedBy(plainWorker.Capabilities)).IsFalse();
        await Assert.That(plainAssignment.RequiredCapabilities.IsSatisfiedBy(plainWorker.Capabilities)).IsTrue();
    }

    [Test]
    public async Task Alternative_Requirement_Routes_To_Any_Matching_Worker()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment
        {
            ModuleId = "Unix.Module",
            RequiredCapabilities = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS),
            AlwaysRun = false,
            PipelineSchemaVersion = string.Empty,
        };

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
        await Assert.That(async () => await coordinator.DequeueModuleAsync(
                DistributedTestData.Worker,
                new HashSet<Capability> { Capability.Windows },
                cts.Token))
            .Throws<OperationCanceledException>();
        var macResult = await coordinator.DequeueModuleAsync(
            DistributedTestData.Worker,
            new HashSet<Capability> { Capability.MacOS },
            CancellationToken.None);

        await Assert.That(macResult?.Assignment.ModuleId).IsEqualTo("Unix.Module");
    }
}
