using System.IO.Compression;
using System.Reflection;
using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedWorkPublisher(
    IDistributedMasterCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry? dependencyRegistry = null,
    IModuleMetadataRegistry? metadataRegistry = null,
    DistributedConditionRouting? conditionRouting = null,
    IModuleConditionHandler? conditionHandler = null)
{
    private readonly IDistributedMasterCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry? _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry? _metadataRegistry = metadataRegistry;
    private readonly DistributedConditionRouting? _conditionRouting = conditionRouting;
    private readonly IModuleConditionHandler? _conditionHandler = conditionHandler;

    public async Task<ModuleAssignment> CreateAssignmentAsync(
        IModule module,
        CancellationToken cancellationToken)
    {
        if (_conditionHandler is not null)
        {
            await _conditionHandler.PrepareDistributedRoutingAsync(module, cancellationToken)
                .ConfigureAwait(false);
        }

        return CreateAssignment(module);
    }

    public ModuleAssignment CreateAssignment(IModule module)
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

        var dependencyResults = GatherDependencyResults(module);

        return new ModuleAssignment(
            ModuleTypeName: moduleType.FullName!,
            ResultTypeName: resultTypeName,
            RequiredCapabilities: requiredCapabilities,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(
                TimeoutSeconds: config.Timeout is not null ? (int?) config.Timeout.Value.TotalSeconds : null,
                AlwaysRun: config.AlwaysRun
            ),
            DependencyResults: dependencyResults)
        {
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
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken);
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
    /// Prefix marker for GZip-compressed dependency result JSON.
    /// When <c>SerializedJson</c> starts with this prefix, the remainder is a base64-encoded
    /// GZip payload that must be decompressed before JSON deserialization.
    /// </summary>
    internal const string GzipPrefix = "gzip:";

    /// <summary>
    /// Threshold in bytes above which a dependency result's <c>SerializedJson</c> is compressed
    /// using GZip to prevent coordinator payloads from exceeding transport limits (e.g., Redis
    /// 10 MB request cap). Text-heavy results like build output compress at ~10:1 ratio.
    /// </summary>
    private const int CompressionThresholdBytes = 64 * 1024;

    /// <summary>
    /// GZip-compresses a JSON string and returns it as a prefixed base64 string.
    /// </summary>
    internal static string CompressJson(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }

        return GzipPrefix + Convert.ToBase64String(output.ToArray());
    }

    /// <summary>
    /// Decompresses a GZip-compressed JSON string (with prefix removed).
    /// </summary>
    internal static string DecompressJson(string compressed)
    {
        var payload = compressed.AsSpan(GzipPrefix.Length);
        var bytes = Convert.FromBase64String(payload.ToString());
        using var input = new MemoryStream(bytes);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return Encoding.UTF8.GetString(output.ToArray());
    }

    /// <summary>
    /// Gathers serialized results for all dependencies resolved by the canonical dependency resolver.
    /// The scheduler guarantees that all dependencies have completed before a module becomes ready,
    /// so all results are guaranteed to be in the registry.
    /// </summary>
    private IReadOnlyList<SerializedModuleResult>? GatherDependencyResults(IModule module)
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

        var results = new List<SerializedModuleResult>(dependencies.Count);
        foreach (var (depType, _) in dependencies)
        {
            var result = _resultRegistry.GetResult(depType);
            if (result is null)
            {
                // Optional dependency that didn't run, or not yet registered — skip
                continue;
            }

            var depResultTypeName = ModuleTypeRegistry.GetResultTypeName(depType) ?? "System.Object";
            var workerIndex = result is ModuleResult { WorkerIndex: { } origin }
                ? origin
                : -1;
            var serialized = _serializer.Serialize(result, depType.FullName!, depResultTypeName, workerIndex);

            // Compress large results to stay within transport payload limits.
            if (serialized.SerializedJson.Length > CompressionThresholdBytes)
            {
                serialized = serialized with { SerializedJson = CompressJson(serialized.SerializedJson) };
            }

            results.Add(serialized);
        }

        return results.Count > 0 ? results : null;
    }
}
