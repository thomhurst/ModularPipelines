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

        bool IsConditionGroupSatisfied(Type conditionGroupType) =>
            _executionLocationContext?.IsConditionGroupSatisfied(module, conditionGroupType) == true;

        var requiredCapabilities = CapabilityConditions.GetModuleRequirement(
            moduleType,
            IsConditionGroupSatisfied,
            conditionGroupType => _executionLocationContext?.IsConditionalRouteRequired(module, conditionGroupType) == true);
        if (!requiredCapabilities.IsSatisfiable)
        {
            // Required conditional routes can contradict each other, for example when the local
            // alternatives of RunIfAny<OnLinux, X> and RunIfAny<OnWindows, X> are both false. No worker
            // can run the module, so let any worker claim it: its own condition evaluation skips it.
            requiredCapabilities = CapabilityConditions.GetModuleRequirement(moduleType, IsConditionGroupSatisfied);
        }

        if (!requiredCapabilities.IsSatisfiable)
        {
            throw new InvalidOperationException(
                $"The module has incompatible operating-system requirements: {requiredCapabilities}.");
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
