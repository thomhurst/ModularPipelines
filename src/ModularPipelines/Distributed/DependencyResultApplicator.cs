using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
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
    /// Fetches referenced dependency results once per run and applies them to local module instances
    /// and the result registry.
    /// This enables <c>GetModule&lt;T&gt;()</c> to resolve cross-process dependencies.
    /// <c>TrySetResult</c> is idempotent — safe if CompletionSource was already set.
    /// </summary>
    public static async Task FetchAndApplyAsync(
        IReadOnlyList<DependencyResultReference> dependencyResultReferences,
        DependencyResultCache resultCache,
        Dictionary<string, IModule> moduleLookup,
        ModuleResultSerializer serializer,
        IModuleResultRegistry resultRegistry,
        ILogger logger)
    {
        foreach (var reference in dependencyResultReferences)
        {
            if (!reference.IsAvailable)
            {
                continue;
            }

            if (!moduleLookup.TryGetValue(reference.ModuleTypeName, out var depModule))
            {
                logger.LogDebug("Dependency module instance not found locally: {ModuleTypeName}", reference.ModuleTypeName);
                continue;
            }

            var serializedResult = await resultCache.GetAsync(reference.ModuleTypeName)
                .ConfigureAwait(false);

            try
            {
                var result = serializer.Deserialize(serializedResult);
                if (result is not null)
                {
                    resultRegistry.RegisterResult(depModule.GetType(), result);
                    ModuleCompletionSourceApplicator.TryApply(depModule, result);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to apply dependency result for {ModuleTypeName}", reference.ModuleTypeName);
            }
        }
    }

    /// <summary>
    /// Publishes a failure result when a module cannot be resolved, preventing the master from hanging.
    /// </summary>
    public static async Task PublishResolutionFailureAsync(
        ModuleAssignment assignment,
        int workerIndex,
        IDistributedWorkerCoordinator coordinator,
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
