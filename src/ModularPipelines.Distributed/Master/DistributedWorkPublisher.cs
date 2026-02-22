using System.Threading.Channels;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedWorkPublisher(
    IDistributedCoordinator coordinator,
    ModuleTypeRegistry typeRegistry)
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;

    public ModuleAssignment CreateAssignment(Type moduleType)
    {
        var resultTypeName = ModuleTypeRegistry.GetResultTypeName(moduleType) ?? "System.Object";

        var requiredCapabilities = moduleType
            .GetCustomAttributes(typeof(RequiresCapabilityAttribute), true)
            .Cast<RequiresCapabilityAttribute>()
            .Select(a => a.Capability)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new ModuleAssignment(
            ModuleTypeName: moduleType.FullName!,
            ResultTypeName: resultTypeName,
            RequiredCapabilities: requiredCapabilities,
            MatrixTarget: null, // Set later by matrix expansion
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(
                TimeoutSeconds: null,
                RetryCount: 0,
                AlwaysRun: false
            )
        );
    }

    public async Task PublishAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken);
    }
}
