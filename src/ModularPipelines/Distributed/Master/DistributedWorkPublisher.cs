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
    IDistributedCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry? dependencyRegistry = null,
    IModuleMetadataRegistry? metadataRegistry = null,
    DistributedConditionRouting? conditionRouting = null,
    IModuleConditionHandler? conditionHandler = null)
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
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
            .Select(a => a.Capability)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var conditionAttributes = moduleType.GetCustomAttributes(true).OfType<IConditionAttribute>().ToArray();
        var operatingSystemRoutes = new List<OperatingSystemConditions.OperatingSystemRoute>();
        foreach (var osCondition in conditionAttributes.Where(static attribute =>
                     attribute is not IGroupedConditionAttribute))
        {
            if (_conditionRouting?.IsLocallySatisfied(module, osCondition.GetType()) == true)
            {
                continue;
            }

            if (OperatingSystemConditions.GetRoute(osCondition) is { } route)
            {
                operatingSystemRoutes.Add(route);
            }
        }

        foreach (var alternatives in conditionAttributes
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            if (_conditionRouting?.IsLocallySatisfied(module, alternatives.Key) == true)
            {
                continue;
            }

            if (OperatingSystemConditions.GetRoute(alternatives) is { } route)
            {
                operatingSystemRoutes.Add(route);
            }
        }

        AddOperatingSystemCapabilities(requiredCapabilities, operatingSystemRoutes);

        var config = module.Configuration;

        var dependencyResults = GatherDependencyResults(module);

        // Distributed workers do not yet consume portable retry configuration.
        return new ModuleAssignment(
            ModuleTypeName: moduleType.FullName!,
            ResultTypeName: resultTypeName,
            RequiredCapabilities: requiredCapabilities,
            MatrixTarget: null, // TODO(matrix): Set by MatrixModuleExpander when wired up
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(
                TimeoutSeconds: config.Timeout is not null ? (int?) config.Timeout.Value.TotalSeconds : null,
                RetryCount: 0,
                AlwaysRun: config.AlwaysRun
            ),
            DependencyResults: dependencyResults)
        {
            SatisfiedConditionGroups = _conditionRouting?.GetLocallySatisfiedGroupNames(module) ?? [],
        };
    }

    public async Task PublishAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken);
    }

    private static void AddOperatingSystemCapabilities(
        ISet<string> requiredCapabilities,
        IReadOnlyList<OperatingSystemConditions.OperatingSystemRoute> routes)
    {
        HashSet<string>? effectiveOperatingSystems = null;
        var strictRoutes = routes.Where(static route => !route.IsConditional).ToArray();
        foreach (var route in strictRoutes)
        {
            if (effectiveOperatingSystems is null)
            {
                effectiveOperatingSystems = route.OperatingSystems
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                effectiveOperatingSystems.IntersectWith(route.OperatingSystems);
            }
        }

        if (effectiveOperatingSystems is { Count: 0 })
        {
            foreach (var route in strictRoutes)
            {
                requiredCapabilities.Add(
                    OperatingSystemConditions.GetCapability(route.OperatingSystems));
            }

            return;
        }

        var hasStrictRoutes = effectiveOperatingSystems is not null;
        foreach (var route in routes.Where(static route => route.IsConditional))
        {
            if (effectiveOperatingSystems is null)
            {
                effectiveOperatingSystems = route.OperatingSystems
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                continue;
            }

            var intersection = effectiveOperatingSystems
                .Intersect(route.OperatingSystems, StringComparer.OrdinalIgnoreCase)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (intersection.Count > 0)
            {
                effectiveOperatingSystems = intersection;
            }
            else if (!hasStrictRoutes)
            {
                return;
            }
        }

        if (effectiveOperatingSystems is { Count: > 0 })
        {
            requiredCapabilities.Add(
                OperatingSystemConditions.GetCapability(effectiveOperatingSystems));
        }
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
            var serialized = _serializer.Serialize(result, depType.FullName!, depResultTypeName, workerIndex: -1);

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
