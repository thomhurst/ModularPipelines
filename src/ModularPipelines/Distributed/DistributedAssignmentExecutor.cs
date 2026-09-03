using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModuleResultFactory = ModularPipelines.Engine.Execution.ModuleResultFactory;

namespace ModularPipelines.Distributed;

internal sealed class DistributedAssignmentExecutor(
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleRunner moduleRunner,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    IDistributedWorkerCoordinator coordinator,
    ILogger logger)
{
    public async Task<IModule?> ExecuteAsync(
        ModuleAssignment assignment,
        Dictionary<string, IModule> moduleLookup,
        int instanceIndex,
        Action<IModule, ModuleAssignment>? configureModule,
        CancellationToken cancellationToken)
    {
        var resolved = typeRegistry.Resolve(assignment.ModuleTypeName);
        if (resolved is null || !moduleLookup.TryGetValue(assignment.ModuleTypeName, out var module))
        {
            logger.LogError(
                "Cannot resolve module assignment {ModuleTypeName}. Publishing failure to prevent master hang.",
                assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(
                    assignment,
                    instanceIndex,
                    coordinator,
                    logger,
                    cancellationToken)
                .ConfigureAwait(false);
            return null;
        }

        if (assignment.DependencyResults is { Count: > 0 })
        {
            DependencyResultApplicator.Apply(
                assignment.DependencyResults,
                moduleLookup,
                serializer,
                resultRegistry,
                logger);
        }

        try
        {
            await ExecuteAndPublishAsync(
                    assignment,
                    module,
                    instanceIndex,
                    configureModule,
                    cancellationToken)
                .ConfigureAwait(false);
            return module;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Module {Module} execution failed on distributed worker {Index}",
                assignment.ModuleTypeName,
                instanceIndex);
            await PublishFailureAsync(
                    assignment,
                    resolved.Value.ResultType,
                    module,
                    exception,
                    instanceIndex,
                    cancellationToken)
                .ConfigureAwait(false);
            return null;
        }
    }

    private async Task ExecuteAndPublishAsync(
        ModuleAssignment assignment,
        IModule module,
        int instanceIndex,
        Action<IModule, ModuleAssignment>? configureModule,
        CancellationToken cancellationToken)
    {
        var moduleType = module.GetType();
        await using var serviceScope = serviceScopeFactory.CreateAsyncScope();
        var moduleLogger = serviceScope.ServiceProvider
            .GetRequiredService<IInternalModuleLoggerAccessor>()
            .GetLogger(moduleType) as IInternalModuleLogger
            ?? throw new InvalidOperationException(
                $"No internal module logger is available for {moduleType.Name}.");
        using var outputScope = new ModuleOutputContextScope(moduleType, moduleLogger);

        try
        {
            if (artifactLifecycleManager is not null)
            {
                await artifactLifecycleManager.DownloadConsumedArtifactsAsync(
                        moduleType,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            configureModule?.Invoke(module, assignment);
            var moduleState = new ModuleState(module, moduleType, WorkerModuleScheduler.Instance);
            ModuleStateDependencyInitializer.Populate(
                moduleState,
                typeRegistry.GetRegisteredModuleTypes(),
                dependencyRegistry,
                metadataRegistry);
            using (DistributedAssignmentExecutionScope.Enter())
            {
                await moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, cancellationToken)
                    .ConfigureAwait(false);
            }

            var result = await module.AsInternal().ResultTask.ConfigureAwait(false);
            var artifactReferences = await TryUploadArtifactsAsync(
                    module,
                    assignment.ModuleTypeName,
                    moduleLogger,
                    cancellationToken)
                .ConfigureAwait(false);
            if (result is null)
            {
                return;
            }

            var serialized = serializer.Serialize(
                result,
                assignment.ModuleTypeName,
                assignment.ResultTypeName,
                instanceIndex);
            if (artifactReferences is not null)
            {
                serialized = serialized with { Artifacts = artifactReferences };
            }

            await coordinator.PublishResultAsync(serialized, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            moduleLogger.SetException(exception);
            throw;
        }
    }

    private async Task<IReadOnlyList<ArtifactReference>?> TryUploadArtifactsAsync(
        IModule module,
        string moduleTypeName,
        IModuleLogger moduleLogger,
        CancellationToken cancellationToken)
    {
        if (artifactLifecycleManager is null)
        {
            return null;
        }

        try
        {
            var artifactReferences = await artifactLifecycleManager.UploadProducedArtifactsAsync(
                    module.GetType(),
                    cancellationToken)
                .ConfigureAwait(false);
            return artifactReferences.Count == 0 ? null : artifactReferences;
        }
        catch (Exception exception)
        {
            moduleLogger.LogError(
                exception,
                "Failed to upload artifacts for module {Module}",
                moduleTypeName);
            return null;
        }
    }

    private async Task PublishFailureAsync(
        ModuleAssignment assignment,
        Type resultType,
        IModule module,
        Exception exception,
        int instanceIndex,
        CancellationToken cancellationToken)
    {
        try
        {
            var failureResult = ModuleResultFactory.CreateException(
                resultType,
                exception,
                new ModuleExecutionContext(module, module.GetType()));
            var serialized = serializer.Serialize(
                failureResult,
                assignment.ModuleTypeName,
                assignment.ResultTypeName,
                instanceIndex);
            await coordinator.PublishResultAsync(serialized, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception publishException)
        {
            logger.LogCritical(
                publishException,
                "Failed to publish failure result for module {Module}; master may hang waiting for this result",
                assignment.ModuleTypeName);
        }
    }
}
