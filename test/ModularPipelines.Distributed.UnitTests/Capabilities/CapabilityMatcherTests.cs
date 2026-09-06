using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.UnitTests.Capabilities;

public class CapabilityMatcherTests
{
    [Test]
    public async Task CanExecute_No_Requirements_Returns_True()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<Capability> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanExecute_Matching_Capabilities_Returns_True()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability> { "docker", "linux" },
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<Capability> { "docker", "linux", "high-memory" },
            RegisteredAt: DateTimeOffset.UtcNow);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanExecute_Missing_Capability_Returns_False()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability> { "docker" },
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<Capability> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CanExecute_Case_Insensitive()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<Capability> { "Docker" },
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<Capability> { "docker" },
            RegisteredAt: DateTimeOffset.UtcNow);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }
}
