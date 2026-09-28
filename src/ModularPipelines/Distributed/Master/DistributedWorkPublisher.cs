using System.Diagnostics;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
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
    DistributedTelemetryTracker? telemetryTracker = null,
    AcceptedArtifactRegistry? acceptedArtifacts = null)
{
    private readonly IDistributedMasterCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry? _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry? _metadataRegistry = metadataRegistry;
    private readonly IExecutionLocationContext? _executionLocationContext = executionLocationContext;
    private readonly IModuleConditionHandler? _conditionHandler = conditionHandler;
    private readonly AcceptedArtifactRegistry? _acceptedArtifacts = acceptedArtifacts;

    public async Task<ModuleAssignment> CreateAssignmentAsync(
        IModule module,
        CancellationToken cancellationToken,
        ModulePriority? priority = null,
        TimeSpan criticalPathWeight = default,
        IReadOnlyCollection<IModule>? plannedModules = null)
    {
        if (_conditionHandler is not null)
        {
            await _conditionHandler.PrepareExecutionRoutingAsync(module, cancellationToken)
                .ConfigureAwait(false);
        }

        return CreateAssignment(module, priority, criticalPathWeight, plannedModules);
    }

    public ModuleAssignment CreateAssignment(
        IModule module,
        ModulePriority? priority = null,
        TimeSpan criticalPathWeight = default,
        IReadOnlyCollection<IModule>? plannedModules = null)
    {
        var moduleType = module.GetType();
        var moduleId = ModuleId.FromType(moduleType);

        // Prefer the requirement the master derived from its own condition values while preparing
        // routing; without preparation, require only what the conditions need whatever they return.
        var requiredCapabilities = (_executionLocationContext?.TryGetPreparedConditionValue(module, out var preparedValue) == true
            ? CapabilityConditions.Combine(moduleType, preparedValue)
            : CapabilityConditions.GetModuleRequirement(
                moduleType,
                conditionGroupType =>
                    _executionLocationContext?.IsConditionGroupSatisfied(module, conditionGroupType) == true)) ?? throw new UnsatisfiableModuleRequirementException(moduleType);
        var config = module.Configuration;

        var dependencyResultReferences = GatherDependencyResultReferences(module);

        return new ModuleAssignment
        {
            ModuleId = moduleId,
            RequiredCapabilities = requiredCapabilities,
            AlwaysRun = config.AlwaysRun,
            DependencyResultReferences = dependencyResultReferences,
            RequiredArtifacts = GetRequiredArtifacts(moduleType, plannedModules),
            ConsumedArtifacts = GetConsumedArtifacts(moduleType),
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
    private IReadOnlyList<DependencyResultReference> GatherDependencyResultReferences(IModule module)
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
            return [];
        }

        var references = new List<DependencyResultReference>(dependencies.Count);
        foreach (var (depType, _) in dependencies)
        {
            references.Add(new DependencyResultReference
            {
                ModuleId = ModuleId.FromType(depType),
                IsAvailable = _resultRegistry.GetResult(depType) is not null,
            });
        }

        return references;
    }

    /// <summary>
    /// Gets the artifacts of this producer that planned, not-yet-completed consumers need. Only
    /// consumers without run conditions count, because conditions evaluated on workers could still
    /// skip them; standalone execution applies the same rule to runnable consumers.
    /// </summary>
    private IReadOnlyList<string> GetRequiredArtifacts(
        Type producerType,
        IReadOnlyCollection<IModule>? plannedModules)
    {
        if (plannedModules is null)
        {
            return [];
        }

        var producedNames = producerType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Select(static attribute => attribute.Name)
            .ToHashSet(StringComparer.Ordinal);
        if (producedNames.Count == 0)
        {
            return [];
        }

        return plannedModules
            .Where(consumer => _resultRegistry.GetResult(consumer.GetType()) is null
                               && consumer.Configuration.SkipCondition is null
                               && !consumer.GetType().GetCustomAttributes(inherit: true).OfType<IConditionAttribute>().Any())
            .SelectMany(consumer => consumer.GetType()
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>())
            .Where(consumed => consumed.ProducerModule == producerType && producedNames.Contains(consumed.ArtifactName))
            .Select(static consumed => consumed.ArtifactName)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Gets the accepted artifact references of every producer this module consumes from.
    /// </summary>
    private IReadOnlyList<ArtifactReference> GetConsumedArtifacts(Type consumerType)
    {
        if (_acceptedArtifacts is null)
        {
            return [];
        }

        var consumed = new List<ArtifactReference>();
        foreach (var producer in consumerType
                     .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                     .Cast<ConsumesArtifactAttribute>()
                     .Select(static attribute => ModuleId.FromType(attribute.ProducerModule))
                     .Distinct())
        {
            if (_acceptedArtifacts.TryGet(producer, out var artifacts))
            {
                consumed.AddRange(artifacts);
            }
        }

        return consumed;
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
