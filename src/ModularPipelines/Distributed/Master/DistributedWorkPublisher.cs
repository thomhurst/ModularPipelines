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
    DistributedConditionRouting? conditionRouting = null,
    IModuleConditionHandler? conditionHandler = null)
{
    private readonly IDistributedMasterCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry? _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry? _metadataRegistry = metadataRegistry;
    private readonly DistributedConditionRouting? _conditionRouting = conditionRouting;
    private readonly IModuleConditionHandler? _conditionHandler = conditionHandler;

    public async Task<ModuleAssignment> CreateAssignmentAsync(
        IModule module,
        CancellationToken cancellationToken,
        ModulePriority? priority = null,
        TimeSpan criticalPathWeight = default)
    {
        if (_conditionHandler is not null)
        {
            await _conditionHandler.PrepareDistributedRoutingAsync(module, cancellationToken)
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
        var resultTypeName = ModuleTypeRegistry.GetResultTypeName(moduleType) ?? "System.Object";

        var requiredCapabilities = moduleType
            .GetCustomAttributes(typeof(RequiresCapabilityAttribute), true)
            .Cast<RequiresCapabilityAttribute>()
            .SelectMany(static attribute => attribute.Capabilities)
            .Select(static name => new Capability(name))
            .ToHashSet();

        var conditionAttributes = moduleType.GetCustomAttributes(true).OfType<IConditionAttribute>().ToArray();
        var operatingSystemRoutes = GetOperatingSystemRoutes(module, conditionAttributes);
        AddExplicitOperatingSystemRoutes(requiredCapabilities, operatingSystemRoutes);
        AddOperatingSystemCapabilities(requiredCapabilities, operatingSystemRoutes);

        var config = module.Configuration;

        var dependencyResultReferences = GatherDependencyResultReferences(module);

        return new ModuleAssignment(
            ModuleTypeName: moduleType.FullName!,
            ResultTypeName: resultTypeName,
            RequiredCapabilities: requiredCapabilities,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(
                TimeoutSeconds: config.Timeout is not null ? (int?) config.Timeout.Value.TotalSeconds : null,
                AlwaysRun: config.AlwaysRun
            ),
            DependencyResultReferences: dependencyResultReferences)
        {
            Priority = priority
                       ?? config.Priority
                       ?? moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true)?.Priority
                       ?? ModulePriority.Normal,
            CriticalPathWeight = criticalPathWeight,
            SatisfiedConditionGroups = _conditionRouting?.GetLocallySatisfiedGroupNames(module) ?? [],
        };
    }

    private List<OperatingSystemConditions.OperatingSystemRoute> GetOperatingSystemRoutes(
        IModule module,
        IReadOnlyList<IConditionAttribute> conditionAttributes)
    {
        var operatingSystemRoutes = new List<OperatingSystemConditions.OperatingSystemRoute>();
        AddUngroupedOperatingSystemRoutes(module, conditionAttributes, operatingSystemRoutes);
        AddGroupedOperatingSystemRoutes(module, conditionAttributes, operatingSystemRoutes);
        return operatingSystemRoutes;
    }

    private void AddUngroupedOperatingSystemRoutes(
        IModule module,
        IEnumerable<IConditionAttribute> conditionAttributes,
        ICollection<OperatingSystemConditions.OperatingSystemRoute> operatingSystemRoutes)
    {
        foreach (var osCondition in conditionAttributes.Where(static attribute =>
                     attribute is not IGroupedConditionAttribute))
        {
            if (_conditionRouting?.IsLocallySatisfied(module, osCondition.GetType()) == true)
            {
                continue;
            }

            if (OperatingSystemConditions.GetRoute(osCondition) is { IsConditional: false } route)
            {
                operatingSystemRoutes.Add(route);
            }
        }
    }

    private void AddGroupedOperatingSystemRoutes(
        IModule module,
        IEnumerable<IConditionAttribute> conditionAttributes,
        ICollection<OperatingSystemConditions.OperatingSystemRoute> operatingSystemRoutes)
    {
        foreach (var alternatives in conditionAttributes
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            if (_conditionRouting?.IsLocallySatisfied(module, alternatives.Key) == true)
            {
                continue;
            }

            var alternativeArray = alternatives.ToArray();
            if (OperatingSystemConditions.GetRoute(alternativeArray) is { IsConditional: false } route)
            {
                operatingSystemRoutes.Add(route);
            }
        }
    }

    public async Task PublishAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken).ConfigureAwait(false);
    }

    private static void AddExplicitOperatingSystemRoutes(
        ISet<Capability> requiredCapabilities,
        ICollection<OperatingSystemConditions.OperatingSystemRoute> routes)
    {
        foreach (var capability in requiredCapabilities.ToArray())
        {
            if (!OperatingSystemConditions.TryGetCapabilityRoute(capability, out var route))
            {
                continue;
            }

            requiredCapabilities.Remove(capability);
            routes.Add(route);
        }
    }

    private static void AddOperatingSystemCapabilities(
        ISet<Capability> requiredCapabilities,
        IReadOnlyList<OperatingSystemConditions.OperatingSystemRoute> routes)
    {
        var effectiveOperatingSystems = IntersectRoutes(routes);
        if (effectiveOperatingSystems is { Count: 0 })
        {
            throw new InvalidOperationException(
                "The module has incompatible operating-system requirements.");
        }

        if (effectiveOperatingSystems is { Count: > 0 })
        {
            requiredCapabilities.Add(
                OperatingSystemConditions.GetCapability(effectiveOperatingSystems));
        }
    }

    private static HashSet<string>? IntersectRoutes(
        IEnumerable<OperatingSystemConditions.OperatingSystemRoute> routes)
    {
        HashSet<string>? intersection = null;
        foreach (var route in routes)
        {
            if (intersection is null)
            {
                intersection = route.OperatingSystems
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                intersection.IntersectWith(route.OperatingSystems);
            }
        }

        return intersection;
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
                depType.FullName!,
                _resultRegistry.GetResult(depType) is not null));
        }

        return references;
    }
}
