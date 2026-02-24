using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

/// <summary>
/// Shared logic for applying serialized dependency results to local module instances.
/// Used by both <see cref="DistributedModuleExecutor"/> and <see cref="Worker.WorkerModuleExecutor"/>.
/// </summary>
internal static class DependencyResultApplicator
{
    /// <summary>
    /// Builds an O(1) lookup from module type name to module instance.
    /// </summary>
    public static Dictionary<string, IModule> BuildModuleLookup(IReadOnlyList<IModule> modules)
    {
        var lookup = new Dictionary<string, IModule>(modules.Count, StringComparer.Ordinal);
        foreach (var module in modules)
        {
            var fullName = module.GetType().FullName;
            if (fullName is not null)
            {
                lookup[fullName] = module;
            }
        }

        return lookup;
    }

    /// <summary>
    /// Applies dependency results received in an assignment to local module instances.
    /// This enables <c>GetModule&lt;T&gt;()</c> to resolve cross-process dependencies.
    /// <c>TrySetResult</c> is idempotent — safe if CompletionSource was already set.
    /// </summary>
    public static void Apply(
        IReadOnlyList<SerializedModuleResult> dependencyResults,
        Dictionary<string, IModule> moduleLookup,
        ModuleResultSerializer serializer,
        ILogger logger)
    {
        foreach (var serializedDep in dependencyResults)
        {
            if (!moduleLookup.TryGetValue(serializedDep.ModuleTypeName, out var depModule))
            {
                logger.LogDebug("Dependency module instance not found locally: {ModuleTypeName}", serializedDep.ModuleTypeName);
                continue;
            }

            try
            {
                // Decompress GZip-compressed dependency results before deserialization
                var toDeserialize = serializedDep;
                if (serializedDep.SerializedJson.StartsWith(DistributedWorkPublisher.GzipPrefix, StringComparison.Ordinal))
                {
                    var decompressed = DistributedWorkPublisher.DecompressJson(serializedDep.SerializedJson);
                    toDeserialize = serializedDep with { SerializedJson = decompressed };
                }

                var result = serializer.Deserialize(toDeserialize);
                if (result is not null)
                {
                    ModuleCompletionSourceApplicator.TryApply(depModule, result);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to apply dependency result for {ModuleTypeName}", serializedDep.ModuleTypeName);
            }
        }
    }

    /// <summary>
    /// Publishes a failure result when a module cannot be resolved, preventing the master from hanging.
    /// </summary>
    public static async Task PublishResolutionFailureAsync(
        ModuleAssignment assignment,
        int workerIndex,
        IDistributedCoordinator coordinator,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        try
        {
            var failureResult = new SerializedModuleResult(
                ModuleTypeName: assignment.ModuleTypeName,
                ResultTypeName: assignment.ResultTypeName,
                WorkerIndex: workerIndex,
                SerializedJson: "null",
                CompletedAt: DateTimeOffset.UtcNow);
            await coordinator.PublishResultAsync(failureResult, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex,
                "Failed to publish resolution failure for {Module} — master may hang waiting for this result",
                assignment.ModuleTypeName);
        }
    }
}
