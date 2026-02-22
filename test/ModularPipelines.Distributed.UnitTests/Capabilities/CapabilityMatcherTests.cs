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
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanExecute_Matching_Capabilities_Returns_True()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker", "linux" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "docker", "linux", "high-memory" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanExecute_Missing_Capability_Returns_False()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CanExecute_Case_Insensitive()
    {
        var assignment = new ModuleAssignment(
            ModuleTypeName: "Test.Module",
            ResultTypeName: "System.String",
            RequiredCapabilities: new HashSet<string> { "Docker" },
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));

        var worker = new WorkerRegistration(
            WorkerIndex: 1,
            Capabilities: new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "docker" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Active,
            CurrentModule: null);

        var result = CapabilityMatcher.CanExecute(assignment, worker);

        await Assert.That(result).IsTrue();
    }
}
