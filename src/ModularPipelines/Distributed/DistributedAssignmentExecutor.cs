using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModuleResultFactory = ModularPipelines.Engine.Execution.ModuleResultFactory;

namespace ModularPipelines.Distributed;

/// <summary>
/// Executes one claimed lease in this process and publishes its terminal result. Shared by the
/// master's local worker loop and by worker processes.
/// </summary>
internal sealed class DistributedAssignmentExecutor(
    IDistributedWorkerCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleRunner moduleRunner,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    AcceptedArtifactRegistry? acceptedArtifacts,
    IExecutionLocationContext? executionLocationContext,
    ILogger logger)
{
    /// <summary>
    /// Executes the lease and publishes its result.
    /// </summary>
    /// <returns>The claimed local module, or <see langword="null"/> when it could not be resolved.</returns>
    public async Task<IModule?> ExecuteAsync(
        ModuleLease lease,
        DateTimeOffset claimedAt,
        Dictionary<ModuleId, IModule> moduleLookup,
        DependencyResultCache dependencyResultCache,
        CancellationToken cancellationToken)
    {
        var assignment = lease.Assignment;
        var executionTimer = new DistributedModuleExecutionTimer(claimedAt);
        if (await DependencyResultApplicator.RejectSchemaMismatchAsync(
                    lease,
                    typeRegistry,
                    serializer,
                    coordinator,
                    executionTimer,
                    schemaMismatch => RecordRejectedClaim(assignment, moduleLookup, schemaMismatch))
                .ConfigureAwait(false))
        {
            return moduleLookup.GetValueOrDefault(assignment.ModuleId);
        }

        var resolved = typeRegistry.Resolve(assignment.ModuleId);
        if (resolved is null || !moduleLookup.TryGetValue(assignment.ModuleId, out var module))
        {
            var reason = resolved is null
                ? "its module type is not registered in this process"
                : "no module instance is registered in this process";
            logger.LogError(
                "Cannot execute distributed module {ModuleId}: {Reason}. Publishing a failure result.",
                assignment.ModuleId,
                reason);
            await DependencyResultApplicator.PublishResolutionFailureAsync(
                    lease,
                    reason,
                    coordinator,
                    serializer,
                    logger,
                    executionTimer)
                .ConfigureAwait(false);
            return null;
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            acceptedArtifacts?.RecordConsumed(assignment.ConsumedArtifacts);
            if (assignment.DependencyResultReferences.Count > 0)
            {
                await DependencyResultApplicator.FetchAndApplyAsync(
                        assignment.DependencyResultReferences,
                        dependencyResultCache,
                        moduleLookup,
                        serializer,
                        resultRegistry,
                        logger,
                        executionTimer,
                        acceptedArtifacts: acceptedArtifacts)
                    .ConfigureAwait(false);
            }

            await ExecuteAndPublishAsync(lease, module, executionTimer, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            if (WorkerCancellationClassifier.IsExpected(exception, cancellationToken))
            {
                logger.LogDebug(exception, "Distributed module {Module} was cancelled", assignment.ModuleId);
            }
            else
            {
                logger.LogError(exception, "Distributed module {Module} failed", assignment.ModuleId);
            }

            await PublishFailureAsync(lease, resolved.Value.ResultType, module, exception, executionTimer)
                .ConfigureAwait(false);
        }

        return module;
    }

    private async Task ExecuteAndPublishAsync(
        ModuleLease lease,
        IModule module,
        DistributedModuleExecutionTimer executionTimer,
        CancellationToken cancellationToken)
    {
        var assignment = lease.Assignment;
        var moduleType = module.GetType();
        var serviceScope = serviceScopeFactory.CreateAsyncScope();
        await using var serviceScopeLifetime = serviceScope.ConfigureAwait(false);
        var moduleLogger = serviceScope.ServiceProvider
            .GetRequiredService<IInternalModuleLoggerAccessor>()
            .GetLogger(moduleType) as IInternalModuleLogger
            ?? throw new InvalidOperationException($"No internal module logger is available for {moduleType.Name}.");
        using var outputScope = new ModuleOutputContextScope(moduleType, moduleLogger);

        try
        {
            if (artifactLifecycleManager is not null)
            {
                var downloadStartedAt = Stopwatch.GetTimestamp();
                try
                {
                    await artifactLifecycleManager.DownloadConsumedArtifactsAsync(moduleType, cancellationToken)
                        .ConfigureAwait(false);
                }
                finally
                {
                    executionTimer.ArtifactDownloadDuration = Stopwatch.GetElapsedTime(downloadStartedAt);
                }
            }

            var moduleState = new ModuleState(module, moduleType);
            executionLocationContext?.RestoreSatisfiedConditionGroups(module, assignment.SatisfiedConditionGroups);
            ModuleStateDependencyInitializer.Populate(
                moduleState,
                typeRegistry.GetRegisteredModuleTypes(),
                dependencyRegistry,
                metadataRegistry);
            IModuleResult result;
            executionTimer.StartExecution();
            try
            {
                using (DistributedAssignmentExecutionScope.Enter())
                {
                    await moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, cancellationToken)
                        .ConfigureAwait(false);
                }

                result = await module.AsInternal().ResultTask.ConfigureAwait(false);
            }
            finally
            {
                executionTimer.FinishExecution();
            }

            IReadOnlyList<ArtifactReference> artifacts;
            var uploadStartedAt = Stopwatch.GetTimestamp();
            try
            {
                artifacts = await UploadArtifactsAsync(assignment, moduleType, result, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception) when (!WorkerCancellationClassifier.IsExpected(exception, cancellationToken))
            {
                // Standalone execution fails a module whose artifacts cannot be uploaded; do the same
                // here instead of reporting success to consumers that will not find the artifact.
                var failure = new ModuleFailedException(moduleType, exception);
                moduleLogger.LogError(exception, "Failed to upload artifacts for module {Module}", assignment.ModuleId);
                await PublishReplacementFailureAsync(lease, module, failure, executionTimer).ConfigureAwait(false);
                return;
            }
            finally
            {
                executionTimer.ArtifactUploadDuration = Stopwatch.GetElapsedTime(uploadStartedAt);
            }

            var serialized = serializer.Serialize(result, assignment.ModuleId, lease.WorkerId) with
            {
                Artifacts = artifacts,
                ExecutionTelemetry = executionTimer.CreateTelemetry(),
            };
            acceptedArtifacts?.Record(serialized);
            await coordinator.PublishResultAsync(serialized, lease, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            moduleLogger.SetException(ex);
            throw;
        }
    }

    private async Task<IReadOnlyList<ArtifactReference>> UploadArtifactsAsync(
        ModuleAssignment assignment,
        Type moduleType,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        if (artifactLifecycleManager is null
            || result.Status is not (ModuleStatus.Succeeded or ModuleStatus.RestoredFromCache or ModuleStatus.RestoredFromHistory))
        {
            return [];
        }

        var artifacts = await artifactLifecycleManager.UploadProducedArtifactsAsync(moduleType, cancellationToken)
            .ConfigureAwait(false);
        ArtifactLifecycleManager.EnsureRequiredArtifactsProduced(moduleType, artifacts, assignment.RequiredArtifacts);
        return artifacts;
    }

    private async Task PublishFailureAsync(
        ModuleLease lease,
        Type resultType,
        IModule module,
        Exception exception,
        DistributedModuleExecutionTimer executionTimer)
    {
        var assignment = lease.Assignment;
        try
        {
            var resultTask = module.AsInternal().ResultTask;
            // A transport failure cannot replace an outcome already accepted by the module.
            var terminalResult = resultTask.IsCompletedSuccessfully
                ? resultTask.Result
                : ModuleResultFactory.CreateException(
                    resultType,
                    exception,
                    new ModuleExecutionContext(module, module.GetType())
                    {
                        Status = exception is OperationCanceledException ? ModuleStatus.Cancelled : ModuleStatus.Failed,
                        Exception = exception,
                    });

            // Record the failure locally too, so this process's summary reports the module.
            new ExecutionBackendContext(resultRegistry).TryApplyResult(module, terminalResult);
            SerializedModuleResult serialized;
            try
            {
                serialized = serializer.Serialize(terminalResult, assignment.ModuleId, lease.WorkerId);
            }
            catch (Exception serializationException) when (resultTask.IsCompletedSuccessfully)
            {
                // An accepted outcome that cannot cross the wire must still complete the master's waiter.
                await PublishReplacementFailureAsync(lease, module, serializationException, executionTimer)
                    .ConfigureAwait(false);
                return;
            }

            serialized = serialized with { ExecutionTelemetry = executionTimer.CreateTelemetry() };
            await DistributedFailurePublisher.PublishAsync(coordinator, serialized, lease).ConfigureAwait(false);
        }
        catch (Exception publishException)
        {
            logger.LogCritical(
                publishException,
                "Failed to publish failure result for module {Module}; the coordinator requeues it when its lease expires",
                assignment.ModuleId);
        }
    }

    /// <summary>
    /// Publishes a failure for a module whose local result already completed, recording the failure
    /// in this process's result registry because the module's completion source cannot change.
    /// </summary>
    private async Task PublishReplacementFailureAsync(
        ModuleLease lease,
        IModule module,
        Exception exception,
        DistributedModuleExecutionTimer executionTimer)
    {
        var moduleType = module.GetType();
        var failure = ModuleResultFactory.CreateException(
            module.ResultType,
            exception,
            new ModuleExecutionContext(module, moduleType)
            {
                Status = ModuleStatus.Failed,
                Exception = exception,
            });
        resultRegistry.RegisterResult(moduleType, failure);
        var serialized = serializer.Serialize(failure, lease.Assignment.ModuleId, lease.WorkerId) with
        {
            ExecutionTelemetry = executionTimer.CreateTelemetry(),
        };
        await DistributedFailurePublisher.PublishAsync(coordinator, serialized, lease).ConfigureAwait(false);
    }

    /// <summary>
    /// Records a claim rejected before execution as a local failure, so this process's results and
    /// summary include it. A claim whose module has no instance in this process cannot be recorded.
    /// </summary>
    private void RecordRejectedClaim(
        ModuleAssignment assignment,
        Dictionary<ModuleId, IModule> moduleLookup,
        Exception exception)
    {
        if (!moduleLookup.TryGetValue(assignment.ModuleId, out var module))
        {
            return;
        }

        var failure = ModuleResultFactory.CreateException(
            module.ResultType,
            exception,
            new ModuleExecutionContext(module, module.GetType())
            {
                Status = ModuleStatus.Failed,
                Exception = exception,
            });
        new ExecutionBackendContext(resultRegistry).TryApplyResult(module, failure);
    }
}
