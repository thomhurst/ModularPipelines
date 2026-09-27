using System.Diagnostics;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedWorkPublisher(
    IDistributedMasterCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry? dependencyRegistry = null,
    IModuleMetadataRegistry? metadataRegistry = null,
    IExecutionLocationContext? executionLocationContext = null,
    IModuleConditionHandler? conditionHandler = null,
    DistributedTelemetryTracker? telemetryTracker = null)
{
    private readonly IDistributedMasterCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry? _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry? _metadataRegistry = metadataRegistry;
    private readonly IExecutionLocationContext? _executionLocationContext = executionLocationContext;
    private readonly IModuleConditionHandler? _conditionHandler = conditionHandler;

    public async Task<ModuleAssignment> CreateAssignmentAsync(
        IModule module,
        CancellationToken cancellationToken,
        ModulePriority? priority = null,
        TimeSpan criticalPathWeight = default)
    {
        if (_conditionHandler is not null)
        {
            await _conditionHandler.PrepareExecutionRoutingAsync(module, cancellationToken)
                .ConfigureAwait(false);
        }

        return CreateAssignment(module, priority, criticalPathWeight);
    }

    public ModuleAssignment CreateAssignment(
        IModule module,
        ModulePriority? priority = null,
        TimeSpan criticalPathWeight = default)
    {
        var moduleType = module.GetType();
        var moduleId = ModuleId.FromType(moduleType);

        // Prefer the requirement the master derived from its own condition values while preparing
        // routing; without preparation, require only what the conditions need whatever they return.
        var requiredCapabilities = _executionLocationContext?.TryGetPreparedConditionValue(module, out var preparedValue) == true
            ? CapabilityConditions.Combine(moduleType, preparedValue)
            : CapabilityConditions.GetModuleRequirement(
                moduleType,
                conditionGroupType =>
                    _executionLocationContext?.IsConditionGroupSatisfied(module, conditionGroupType) == true);
        if (requiredCapabilities is null)
        {
            throw new UnsatisfiableModuleRequirementException(moduleType);
        }

        var config = module.Configuration;

        var dependencyResultReferences = GatherDependencyResultReferences(module);

        return new ModuleAssignment(
            ModuleId: moduleId,
            RequiredCapabilities: requiredCapabilities,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentOptions(
                Timeout: config.Timeout,
                AlwaysRun: config.AlwaysRun
            ),
            DependencyResultReferences: dependencyResultReferences)
        {
            Priority = priority
                       ?? config.Priority
                       ?? moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true)?.Priority
                       ?? ModulePriority.Normal,
            CriticalPathWeight = criticalPathWeight,
            PipelineSchemaVersion = _typeRegistry.GetPipelineSchemaVersion(),
            SatisfiedConditionGroups = _executionLocationContext?.GetSatisfiedConditionGroupNames(module) ?? [],
        };
    }

    public async Task PublishAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        assignment = assignment with { EnqueuedAt = DateTimeOffset.UtcNow };
        var startedAt = Stopwatch.GetTimestamp();
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken).ConfigureAwait(false);
        telemetryTracker?.RecordAssignment(assignment, Stopwatch.GetElapsedTime(startedAt));
    }

    /// <summary>
    /// Gathers result-store references for all dependencies resolved by the canonical dependency resolver.
    /// </summary>
    private IReadOnlyList<DependencyResultReference>? GatherDependencyResultReferences(IModule module)
    {
        var dependencies = ModuleDependencyResolver
            .GetAllDependencies(
                module,
                _typeRegistry.GetRegisteredModuleTypes(),
                _dependencyRegistry,
                _metadataRegistry)
            .DistinctBy(dependency => dependency.DependencyType)
            .ToList();
        if (dependencies.Count == 0)
        {
            return null;
        }

        var references = new List<DependencyResultReference>(dependencies.Count);
        foreach (var (depType, _) in dependencies)
        {
            references.Add(new DependencyResultReference(
                ModuleId.FromType(depType),
                _resultRegistry.GetResult(depType) is not null));
        }

        return references;
    }
}

/// <summary>
/// Thrown when no worker can satisfy a module's run conditions and capability requirements, so the
/// module must be skipped instead of dispatched.
/// </summary>
internal sealed class UnsatisfiableModuleRequirementException(Type moduleType)
    : InvalidOperationException(
        $"No worker can run {moduleType.Name}: its capability requirements and run conditions are " +
        "incompatible (incompatible operating-system requirements or a condition that is false on the master).")
{
    public SkipDecision SkipDecision { get; } = SkipDecision.Skip(
        "No worker can satisfy the module's capability requirements and run conditions");
}
