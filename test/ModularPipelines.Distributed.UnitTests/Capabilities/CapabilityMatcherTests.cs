using ModularPipelines.Distributed.Capabilities;

namespace ModularPipelines.Distributed.UnitTests.Capabilities;

public class CapabilityMatcherTests
{
    [Test]
    public async Task CanExecute_No_Requirements_Returns_True()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: [],
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: [ "linux" ],
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
            RequiredCapabilities: [ "docker", "linux" ],
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: [ "docker", "linux", "high-memory" ],
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
            RequiredCapabilities: [ "docker" ],
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: [ "linux" ],
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
            RequiredCapabilities: [ "Docker" ],
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(null, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: [ "docker" ],
            RegisteredAt: DateTimeOffset.UtcNow);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }
}
