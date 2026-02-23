using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedWorkPublisher(
    IDistributedCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleResultRegistry resultRegistry)
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;

    public ModuleAssignment CreateAssignment(IModule module)
    {
        var moduleType = module.GetType();
        var resultTypeName = ModuleTypeRegistry.GetResultTypeName(moduleType) ?? "System.Object";

        var requiredCapabilities = moduleType
            .GetCustomAttributes(typeof(RequiresCapabilityAttribute), true)
            .Cast<RequiresCapabilityAttribute>()
            .Select(a => a.Capability)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var config = module.Configuration;

        var dependencyResults = GatherDependencyResults(moduleType);

        return new ModuleAssignment(
            ModuleTypeName: moduleType.FullName!,
            ResultTypeName: resultTypeName,
            RequiredCapabilities: requiredCapabilities,
            MatrixTarget: null, // TODO(matrix): Set by MatrixModuleExpander when wired up
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(
                TimeoutSeconds: config.Timeout is not null ? (int?)config.Timeout.Value.TotalSeconds : null,
                RetryCount: 0, // Retry policies are Polly IAsyncPolicy factories, not portable across processes
                AlwaysRun: config.AlwaysRun
            ),
            DependencyResults: dependencyResults
        );
    }

    public async Task PublishAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        await _coordinator.EnqueueModuleAsync(assignment, cancellationToken);
    }

    /// <summary>
    /// Maximum size in bytes for a single dependency result's serialized JSON.
    /// Results exceeding this are stripped to metadata-only (Value set to null) to prevent
    /// coordinator payloads from exceeding transport limits (e.g., Redis 10 MB request cap).
    /// The stripped result still satisfies <c>GetModule&lt;T&gt;()</c> — it resolves with a
    /// null value, which is sufficient for dependency barrier semantics.
    /// </summary>
    private const int MaxDependencyResultJsonBytes = 256 * 1024;

    /// <summary>
    /// Gathers serialized results for all dependencies declared via <c>[DependsOn&lt;T&gt;]</c>.
    /// The scheduler guarantees that all dependencies have completed before a module becomes ready,
    /// so all results are guaranteed to be in the registry.
    /// </summary>
    private IReadOnlyList<SerializedModuleResult>? GatherDependencyResults(Type moduleType)
    {
        var dependencies = ModuleDependencyResolver.GetDependencies(moduleType).ToList();
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

            // If the serialized result is too large, strip the Value to null to keep the
            // assignment payload within transport limits. The metadata is preserved so the
            // worker can still resolve GetModule<T>() (it will return a result with null value).
            if (serialized.SerializedJson.Length > MaxDependencyResultJsonBytes)
            {
                serialized = serialized with { SerializedJson = StripValueFromJson(serialized.SerializedJson) };
            }

            results.Add(serialized);
        }

        return results.Count > 0 ? results : null;
    }

    /// <summary>
    /// Replaces the "Value" property in a serialized ModuleResult JSON with null,
    /// preserving all metadata ($type, ModuleName, timing, status).
    /// </summary>
    private static string StripValueFromJson(string json)
    {
        using var doc = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        writer.WriteStartObject();

        foreach (var property in doc.RootElement.EnumerateObject())
        {
            if (property.Name == "Value")
            {
                writer.WriteNull("Value");
            }
            else
            {
                property.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
        writer.Flush();
        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
    }
}
