using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedModuleExecutor(
    IHostApplicationLifetime lifetime,
    IModuleSchedulerFactory schedulerFactory,
    IModuleRunner moduleRunner,
    IRegistrationEventExecutor registrationEventExecutor,
    IDistributedCoordinator coordinator,
    DistributedWorkPublisher publisher,
    DistributedResultCollector resultCollector,
    ModuleTypeRegistry typeRegistry,
    IModuleResultRegistry resultRegistry,
    IOptions<DistributedOptions> options,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<DistributedModuleExecutor> logger) : IModuleExecutor
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IModuleSchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IRegistrationEventExecutor _registrationEventExecutor = registrationEventExecutor;
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly DistributedWorkPublisher _publisher = publisher;
    private readonly DistributedResultCollector _resultCollector = resultCollector;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
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

        // Wait for workers to register before distributing work
        await WaitForWorkersAsync(_lifetime.ApplicationStopping);

        IModuleScheduler? scheduler = null;
        try
        {
            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.ApplicationStopping);
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
                        // TODO(matrix): MatrixModuleExpander.ScanForExpansions not yet connected.
                        // Modules with [MatrixTarget] will run once, not N times.
                        _logger.LogInformation("Distributing module {Module} to workers", moduleType.Name);
                        var assignment = _publisher.CreateAssignment(moduleState.Module);
                        await _publisher.PublishAsync(assignment, cts.Token);

                        var collectTask = CollectDistributedResultAsync(moduleState.Module, moduleType, scheduler, cts);
                        resultTasks.Add(collectTask);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected during shutdown
            }

            try
            {
                await Task.WhenAll(resultTasks).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected when a module failure cancels the pipeline
            }

            if (!cts.IsCancellationRequested)
            {
                await cts.CancelAsync();
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
            // Always signal workers to stop — whether the master succeeded or crashed.
            // Without this, workers hang forever waiting for work that will never come.
            try
            {
                await _coordinator.SignalCompletionAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to signal completion to workers during shutdown");
            }

            scheduler?.Dispose();
        }

        return modules;
    }

    private async Task WaitForWorkersAsync(CancellationToken cancellationToken)
    {
        var expectedWorkers = _options.Value.TotalInstances - 1;
        if (expectedWorkers <= 0)
        {
            return;
        }

        var timeout = TimeSpan.FromSeconds(_options.Value.CapabilityTimeoutSeconds);
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout);

        _logger.LogInformation("Waiting for {Expected} worker(s) to register (timeout: {Timeout}s)...",
            expectedWorkers, _options.Value.CapabilityTimeoutSeconds);

        var lastCount = 0;
        while (!timeoutCts.IsCancellationRequested)
        {
            try
            {
                var workers = await _coordinator.GetRegisteredWorkersAsync(timeoutCts.Token);
                if (workers.Count != lastCount)
                {
                    lastCount = workers.Count;
                    _logger.LogInformation("{Count}/{Expected} worker(s) registered", workers.Count, expectedWorkers);
                }

                if (workers.Count >= expectedWorkers)
                {
                    _logger.LogInformation("All {Expected} worker(s) registered — starting work distribution", expectedWorkers);
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(2), timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                // Timeout expired, but pipeline not cancelled — proceed with available workers
                _logger.LogWarning(
                    "Worker registration timeout ({Timeout}s expired). {Count}/{Expected} worker(s) registered — proceeding with available workers",
                    _options.Value.CapabilityTimeoutSeconds, lastCount, expectedWorkers);
                return;
            }
        }
    }

    private async Task ExecuteLocalWithArtifactsAsync(ModuleState moduleState, Type moduleType, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        // Mark started on the real scheduler for tracking
        scheduler.MarkModuleStarted(moduleType);

        // Execute with a no-op scheduler so ExecuteCore's internal MarkModuleCompleted
        // doesn't release dependent modules before artifacts are uploaded.
        using var localScheduler = new WorkerModuleScheduler();
        try
        {
            await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, localScheduler, cancellationToken);

            // Determine actual success — ExecuteCore may handle failure without throwing
            var success = moduleState.Result is not null && !moduleState.Result.IsFailure;

            // Upload produced artifacts after successful execution, before releasing dependents
            if (success && _artifactLifecycleManager is not null)
            {
                await _artifactLifecycleManager.UploadProducedArtifactsAsync(moduleType, cancellationToken);
            }

            // NOW mark completed on the real scheduler — this releases dependent modules
            scheduler.MarkModuleCompleted(moduleType, success, statusOverride: moduleState.Result?.ModuleStatus);
        }
        catch (Exception ex)
        {
            scheduler.MarkModuleCompleted(moduleType, false, ex);
            throw;
        }
    }

    private async Task CollectDistributedResultAsync(IModule module, Type moduleType, IModuleScheduler scheduler, CancellationTokenSource cts)
    {
        try
        {
            scheduler.MarkModuleStarted(moduleType);
            var result = await _resultCollector.WaitForResultAsync(moduleType.FullName!, cts.Token);
            var success = result is not null && !result.IsFailure;

            // Apply the deserialized result to the module's CompletionSource so that
            // DependsOn<T> result access works across the master/worker boundary
            if (result is not null)
            {
                ApplyResultToModule(module, result);
                _resultRegistry.RegisterResult(moduleType, result);
            }

            scheduler.MarkModuleCompleted(moduleType, success);

            if (!success)
            {
                _logger.LogError("Distributed module {Module} failed on worker — cancelling pipeline", moduleType.Name);
                await cts.CancelAsync();
            }
        }
        catch (OperationCanceledException)
        {
            RegisterFailureResult(module, moduleType, new OperationCanceledException("Module was cancelled"));
            scheduler.MarkModuleCompleted(moduleType, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect result for distributed module {Module}", moduleType.Name);
            RegisterFailureResult(module, moduleType, ex);
            scheduler.MarkModuleCompleted(moduleType, false, ex);
            await cts.CancelAsync();
        }
    }

    private void RegisterFailureResult(IModule module, Type moduleType, Exception exception)
    {
        try
        {
            var failureResult = ModuleResultFactory.CreateException(
                module.ResultType,
                exception,
                new ModuleExecutionContext(module, moduleType));
            _resultRegistry.RegisterResult(moduleType, failureResult);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to register failure result for module {Module}", moduleType.Name);
        }
    }

    /// <summary>
    /// Sets the deserialized result on the module's internal <c>CompletionSource</c> via reflection,
    /// since the generic type parameter T is not known at compile time.
    /// </summary>
    private static void ApplyResultToModule(IModule module, IModuleResult result)
    {
        var completionSource = module.GetType()
            .GetProperty("CompletionSource", BindingFlags.Instance | BindingFlags.NonPublic)?
            .GetValue(module);

        completionSource?.GetType()
            .GetMethod("TrySetResult")?
            .Invoke(completionSource, [result]);
    }
}
