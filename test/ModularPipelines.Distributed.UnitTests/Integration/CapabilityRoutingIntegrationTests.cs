using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CapabilityRoutingIntegrationTests
{
    [Test]
    public async Task Capable_Worker_Receives_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment(
            ModuleId: "Docker.Module",
            RequiredCapabilities: CapabilityRequirement.AllOf("docker"),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker with docker capability
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { "linux", "docker" }, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleId).IsEqualTo("Docker.Module");
    }

    [Test]
    public async Task Incapable_Worker_Does_Not_Receive_Assignment()
    {
        var coordinator = new InMemoryDistributedCoordinator();

        var assignment = new ModuleAssignment(
            ModuleId: "Docker.Module",
            RequiredCapabilities: CapabilityRequirement.AllOf("docker"),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        // Worker without docker capability - should timeout
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
        var result = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task Requirement_Validates_Worker_Assignments()
    {
        var dockerWorker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: ["linux", "docker"],
            RegisteredAt: DateTimeOffset.UtcNow);

        var plainWorker = new WorkerRegistration(
            WorkerIndex: 2,
            Capabilities: ["linux"],
            RegisteredAt: DateTimeOffset.UtcNow);

        var dockerAssignment = new ModuleAssignment(
            ModuleId: "Docker.Module",
            RequiredCapabilities: CapabilityRequirement.AllOf("docker"),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        var plainAssignment = new ModuleAssignment(
            ModuleId: "Plain.Module",
            RequiredCapabilities: CapabilityRequirement.None,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

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

        var assignment = new ModuleAssignment(
            ModuleId: "Unix.Module",
            RequiredCapabilities: CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        await coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
        var windowsResult = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { Capability.Windows }, cts.Token);
        var macResult = await coordinator.DequeueModuleAsync(
            new HashSet<Capability> { Capability.MacOS }, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(windowsResult).IsNull();
            await Assert.That(macResult?.ModuleId).IsEqualTo("Unix.Module");
        }
    }
}
