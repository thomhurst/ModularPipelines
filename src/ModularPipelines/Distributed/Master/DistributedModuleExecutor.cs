using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedModuleExecutor(
    IModuleSchedulerFactory schedulerFactory,
    IModuleRunner moduleRunner,
    IRegistrationEventExecutor registrationEventExecutor,
    IDistributedCoordinator coordinator,
    DistributedWorkPublisher publisher,
    DistributedResultCollector resultCollector,
    ModuleTypeRegistry typeRegistry,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<DistributedModuleExecutor> logger) : IModuleExecutor
{
    private readonly IModuleSchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IRegistrationEventExecutor _registrationEventExecutor = registrationEventExecutor;
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly DistributedWorkPublisher _publisher = publisher;
    private readonly DistributedResultCollector _resultCollector = resultCollector;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<DistributedModuleExecutor> _logger = logger;

    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        if (modules.Count == 0)
        {
            return modules;
        }

        // Register all module types in the type registry for serialization
        foreach (var module in modules)
        {
            _typeRegistry.Register(module.GetType());
        }

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        IModuleScheduler? scheduler = null;
        try
        {
            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules);

            using var cts = new CancellationTokenSource();
            cts.Token.Register(() => scheduler.CancelPendingModules());

            var schedulerTask = scheduler.RunSchedulerAsync(cts.Token);
            var resultTasks = new List<Task>();

            try
            {
                await foreach (var moduleState in scheduler.ReadyModules.ReadAllAsync(cts.Token))
                {
                    var moduleType = moduleState.Module.GetType();
                    var isPinToMaster = moduleType.GetCustomAttributes(typeof(PinToMasterAttribute), true).Length > 0;

                    if (isPinToMaster)
                    {
                        _logger.LogInformation("Executing module {Module} locally (PinToMaster)", moduleType.Name);
                        var localTask = ExecuteLocalWithArtifactsAsync(moduleState, moduleType, scheduler, cts.Token);
                        resultTasks.Add(localTask);
                    }
                    else
                    {
                        _logger.LogInformation("Distributing module {Module} to workers", moduleType.Name);
                        var assignment = _publisher.CreateAssignment(moduleType);
                        await _publisher.PublishAsync(assignment, cts.Token);

                        var collectTask = CollectDistributedResultAsync(moduleType, scheduler, cts.Token);
                        resultTasks.Add(collectTask);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected during shutdown
            }

            await Task.WhenAll(resultTasks).ConfigureAwait(false);

            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }

            try
            {
                await schedulerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
        }
        finally
        {
            scheduler?.Dispose();
        }

        return modules;
    }

    private async Task ExecuteLocalWithArtifactsAsync(ModuleState moduleState, Type moduleType, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        await _moduleRunner.ExecuteAsync(moduleState, scheduler, cancellationToken);

        // Upload produced artifacts after local execution (before marking as completed for workers)
        if (_artifactLifecycleManager is not null)
        {
            try
            {
                await _artifactLifecycleManager.UploadProducedArtifactsAsync(moduleType, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload artifacts for PinToMaster module {Module}", moduleType.Name);
            }
        }
    }

    private async Task CollectDistributedResultAsync(Type moduleType, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        try
        {
            scheduler.MarkModuleStarted(moduleType);
            var result = await _resultCollector.WaitForResultAsync(moduleType.FullName!, cancellationToken);
            var success = result is not null && !result.IsFailure;
            scheduler.MarkModuleCompleted(moduleType, success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect result for distributed module {Module}", moduleType.Name);
            scheduler.MarkModuleCompleted(moduleType, false, ex);
        }
    }
}
