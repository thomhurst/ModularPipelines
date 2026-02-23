using System.IO.Compression;
using System.Reflection;
using System.Text;
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

        // Auto-detect OS requirements from RunOn*Only mandatory conditions
        if (moduleType.GetCustomAttribute<RunOnLinuxOnlyAttribute>(true) is not null)
        {
            requiredCapabilities.Add("linux");
        }

        if (moduleType.GetCustomAttribute<RunOnWindowsOnlyAttribute>(true) is not null)
        {
            requiredCapabilities.Add("windows");
        }

        if (moduleType.GetCustomAttribute<RunOnMacOSOnlyAttribute>(true) is not null)
        {
            requiredCapabilities.Add("macos");
        }

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

            // Compress large results to stay within transport payload limits.
            if (serialized.SerializedJson.Length > CompressionThresholdBytes)
            {
                serialized = serialized with { SerializedJson = CompressJson(serialized.SerializedJson) };
            }

            results.Add(serialized);
        }

        return results.Count > 0 ? results : null;
    }

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
}
