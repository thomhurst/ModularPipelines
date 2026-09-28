using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests;

/// <summary>
/// Builds distributed wire objects for tests.
/// </summary>
internal static class DistributedTestData
{
    public static readonly WorkerId Worker = WorkerId.FromInstanceIndex(1);

    public static ModuleAssignment Assignment(ModuleId moduleId, string schemaVersion = "test-schema") => new()
    {
        ModuleId = moduleId,
        RequiredCapabilities = CapabilityRequirement.None,
        PipelineSchemaVersion = schemaVersion,
    };

    public static ModuleAssignment Assignment(IModule module, ModuleTypeRegistry registry) =>
        Assignment(ModuleId.FromType(module.GetType()), registry.GetPipelineSchemaVersion());

    public static ModuleLease Lease(ModuleAssignment assignment, WorkerId? workerId = null) => new()
    {
        LeaseId = Guid.NewGuid().ToString("N"),
        WorkerId = workerId ?? Worker,
        Assignment = assignment,
    };

    public static SerializedModuleResult Result(ModuleId moduleId, string payload = "{}", WorkerId? workerId = null) => new()
    {
        ModuleId = moduleId,
        WorkerId = workerId ?? Worker,
        Payload = payload,
        CompletedAt = DateTimeOffset.UtcNow,
    };

    public static WorkerRegistration Registration(WorkerId workerId, params Capability[] capabilities) => new()
    {
        WorkerId = workerId,
        Capabilities = capabilities,
        RegisteredAt = DateTimeOffset.UtcNow,
    };

    public static DependencyResultReference Dependency(ModuleId moduleId, bool isAvailable = true) => new()
    {
        ModuleId = moduleId,
        IsAvailable = isAvailable,
    };
}
