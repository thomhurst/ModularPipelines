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
    public static async Task<bool> RejectSchemaMismatchAsync(
        ModuleAssignment assignment,
        ModuleTypeRegistry registry,
        ModuleResultSerializer serializer,
        IDistributedWorkerCoordinator coordinator,
        int workerIndex,
        DistributedModuleExecutionTimer executionTimer)
    {
        try
        {
            PipelineSchemaVersionValidator.Validate(
                registry.GetPipelineSchemaVersion(), assignment.PipelineSchemaVersion, "master assignment");
            return false;
        }
        catch (PipelineSchemaMismatchException exception)
        {
            var failure = serializer.SerializeFailure(assignment.ModuleId, exception, workerIndex) with
            {
                ExecutionTelemetry = executionTimer.CreateTelemetry(),
            };
            await DistributedFailurePublisher.PublishAsync(coordinator, failure).ConfigureAwait(false);
            return true;
        }
    }

    /// <summary>
    /// Builds an O(1) lookup from module identifier to module instance.
    /// </summary>
    public static Dictionary<ModuleId, IModule> BuildModuleLookup(IReadOnlyList<IModule> modules)
    {
        var lookup = new Dictionary<ModuleId, IModule>(modules.Count);
        foreach (var module in modules)
        {
            lookup[ModuleId.FromType(module.GetType())] = module;
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
        Dictionary<ModuleId, IModule> moduleLookup,
        ModuleResultSerializer serializer,
        IModuleResultRegistry resultRegistry,
        ILogger logger,
        DistributedModuleExecutionTimer? executionTimer = null,
        TimeProvider? timeProvider = null)
    {
        var clock = timeProvider ?? TimeProvider.System;
        foreach (var reference in dependencyResultReferences)
        {
            if (!reference.IsAvailable)
            {
                continue;
            }

            if (!moduleLookup.TryGetValue(reference.ModuleId, out var depModule))
            {
                logger.LogDebug("Dependency module instance not found locally: {ModuleId}", reference.ModuleId);
                continue;
            }

            var transferStartedAt = clock.GetTimestamp();
            SerializedModuleResult serializedResult;
            try
            {
                serializedResult = await resultCache.GetAsync(reference.ModuleId)
                    .ConfigureAwait(false);
            }
            finally
            {
                executionTimer?.DependencyResultTransferDuration += clock.GetElapsedTime(transferStartedAt);
            }

            var processingStartedAt = clock.GetTimestamp();
            try
            {
                var result = serializer.Deserialize(serializedResult);
                if (result is not null)
                {
                    var applied = ModuleCompletionSourceApplicator.TryApply(depModule, result);
                    var internalModule = depModule.AsInternal();
                    var acceptedResult = !applied && internalModule.ResultTask.IsCompletedSuccessfully
                        ? internalModule.ResultTask.Result
                        : result;
                    resultRegistry.RegisterResult(depModule.GetType(), acceptedResult);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to apply dependency result for {ModuleId}", reference.ModuleId);
            }
            finally
            {
                executionTimer?.DependencyResultProcessingDuration += clock.GetElapsedTime(processingStartedAt);
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
        DistributedModuleExecutionTimer? executionTimer = null)
    {
        try
        {
            var failureResult = new SerializedModuleResult(
                ModuleId: assignment.ModuleId,
                WorkerIndex: workerIndex,
                Payload: "null",
                CompletedAt: DateTimeOffset.UtcNow)
            {
                ExecutionTelemetry = executionTimer?.CreateTelemetry(),
            };
            await DistributedFailurePublisher.PublishAsync(coordinator, failureResult).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex,
                "Failed to publish resolution failure for {Module} — master may hang waiting for this result",
                assignment.ModuleId);
        }
    }
}
