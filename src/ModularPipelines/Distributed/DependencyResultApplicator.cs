using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts;
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
        ModuleLease lease,
        ModuleTypeRegistry registry,
        ModuleResultSerializer serializer,
        IDistributedWorkerCoordinator coordinator,
        DistributedModuleExecutionTimer executionTimer,
        Action<PipelineSchemaMismatchException>? recordRejection = null)
    {
        var assignment = lease.Assignment;
        try
        {
            PipelineSchemaVersionValidator.Validate(
                registry.GetPipelineSchemaVersion(), assignment.PipelineSchemaVersion, "master assignment");
            return false;
        }
        catch (PipelineSchemaMismatchException exception)
        {
            // Record the rejection before publishing, which can fail or time out.
            recordRejection?.Invoke(exception);
            var failure = serializer.SerializeFailure(assignment.ModuleId, exception, lease.WorkerId) with
            {
                ExecutionTelemetry = executionTimer.CreateTelemetry(),
            };
            await DistributedFailurePublisher.PublishAsync(coordinator, failure, lease).ConfigureAwait(false);
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
    /// The coordinator's final result supersedes any unconfirmed local attempt.
    /// </summary>
    public static async Task FetchAndApplyAsync(
        IReadOnlyList<DependencyResultReference> dependencyResultReferences,
        DependencyResultCache resultCache,
        Dictionary<ModuleId, IModule> moduleLookup,
        ModuleResultSerializer serializer,
        IModuleResultRegistry resultRegistry,
        ILogger logger,
        DistributedModuleExecutionTimer? executionTimer = null,
        TimeProvider? timeProvider = null,
        AcceptedArtifactRegistry? acceptedArtifacts = null)
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
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Dependency module instance not found locally: {ModuleId}", reference.ModuleId);
                }
                continue;
            }

            // Only a coordinator-confirmed result is authoritative. A completed local attempt
            // may have lost its lease before publication. Retain confirmed results after eviction.
            var internalModule = depModule.AsInternal();
            if (internalModule.HasAcceptedDistributedResult)
            {
                var localResult = internalModule.ResultTask;
                var localProcessingStartedAt = clock.GetTimestamp();
                try
                {
                    resultRegistry.RegisterAcceptedResult(depModule.GetType(), localResult.Result);
                }
                finally
                {
                    executionTimer?.DependencyResultProcessingDuration += clock.GetElapsedTime(localProcessingStartedAt);
                }

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

            acceptedArtifacts?.Record(serializedResult);
            var processingStartedAt = clock.GetTimestamp();
            try
            {
                var result = serializer.Deserialize(serializedResult) ?? throw new InvalidOperationException(
                    $"Dependency result for '{reference.ModuleId}' is empty.");
                var acceptedResult = depModule.AsInternal().ApplyAcceptedDistributedResult(result);
                resultRegistry.RegisterAcceptedResult(depModule.GetType(), acceptedResult);
            }
            finally
            {
                executionTimer?.DependencyResultProcessingDuration += clock.GetElapsedTime(processingStartedAt);
            }
        }
    }

    /// <summary>
    /// Publishes a failure result when a claimed module cannot be resolved, so the master records a
    /// descriptive failure instead of waiting or reporting a missing result.
    /// </summary>
    public static async Task PublishResolutionFailureAsync(
        ModuleLease lease,
        string reason,
        IDistributedWorkerCoordinator coordinator,
        ModuleResultSerializer serializer,
        ILogger logger,
        DistributedModuleExecutionTimer? executionTimer = null)
    {
        var moduleId = lease.Assignment.ModuleId;
        try
        {
            var exception = new InvalidOperationException(
                $"Worker {lease.WorkerId} cannot execute distributed module '{moduleId}': {reason}");
            var failureResult = serializer.SerializeFailure(moduleId, exception, lease.WorkerId) with
            {
                ExecutionTelemetry = executionTimer?.CreateTelemetry(),
            };
            await DistributedFailurePublisher.PublishAsync(coordinator, failureResult, lease).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(ex,
                    "Failed to publish resolution failure for {Module}; the coordinator requeues it when its lease expires",
                    moduleId);
            }
        }
    }
}
