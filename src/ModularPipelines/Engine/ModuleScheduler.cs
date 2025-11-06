using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages eager parallel scheduling of modules using channels
/// </summary>
internal class ModuleScheduler : IModuleScheduler
{
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;
    private readonly Channel<ModuleState> _readyChannel;
    private readonly SemaphoreSlim _schedulerNotification;
    private readonly CancellationTokenSource _disposalCancellationTokenSource;
    private readonly object _stateLock = new object();

    private bool _schedulerCompleted;
    private bool _isDisposed;

    public ModuleScheduler(ILogger logger)
    {
        _logger = logger;
        _moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        _readyChannel = Channel.CreateUnbounded<ModuleState>(new UnboundedChannelOptions
        {
            SingleWriter = true,  // Only scheduler writes
            SingleReader = false  // Multiple workers read
        });
        _schedulerNotification = new SemaphoreSlim(0);
        _disposalCancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// Gets the channel reader for consuming ready modules
    /// </summary>
    public ChannelReader<ModuleState> ReadyModules => _readyChannel.Reader;

    /// <summary>
    /// Initializes module states for a collection of modules
    /// </summary>
    public void InitializeModules(IEnumerable<ModuleBase> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        var moduleList = modules.ToList();

        foreach (var module in moduleList)
        {
            var state = new ModuleState(module);
            _moduleStates.TryAdd(module.GetType(), state);
        }

        foreach (var state in _moduleStates.Values)
        {
            var moduleType = state.Module.GetType();

            var notInParallelAttr = moduleType.GetCustomAttribute<NotInParallelAttribute>();
            if (notInParallelAttr != null)
            {
                if (notInParallelAttr.ConstraintKeys.Length == 0)
                {
                    state.RequiresSequentialExecution = true;
                    _logger.LogDebug(
                        "Module {ModuleName} requires sequential execution (NotInParallel)",
                        MarkupFormatter.FormatModuleName(moduleType.Name));
                }
                else
                {
                    state.RequiredLockKeys = notInParallelAttr.ConstraintKeys;
                    _logger.LogDebug(
                        "Module {ModuleName} requires locks: {Keys}",
                        MarkupFormatter.FormatModuleName(moduleType.Name),
                        string.Join(", ", state.RequiredLockKeys));
                }
            }

            var dependencies = state.Module.GetModuleDependencies();

            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                if (_moduleStates.TryGetValue(dependencyType, out var dependencyState))
                {
                    state.UnresolvedDependencies.Add(dependencyType);
                    dependencyState.DependentModules.Add(state);
                }
                else if (!ignoreIfNotRegistered)
                {
                    _logger.LogWarning(
                        "Module {ModuleName} depends on {DependencyName} which is not registered",
                        MarkupFormatter.FormatModuleName(state.Module.GetType().Name),
                        MarkupFormatter.FormatModuleName(dependencyType.Name));
                }
            }
        }

        _logger.LogDebug(
            "Initialized {Count} modules for scheduling with total of {DependencyCount} dependencies",
            _moduleStates.Count,
            _moduleStates.Values.Sum(x => x.UnresolvedDependencies.Count));
    }

    /// <summary>
    /// Adds a new module dynamically (e.g., SubModule discovered during execution)
    /// </summary>
    public void AddModule(ModuleBase module)
    {
        var state = new ModuleState(module);

        if (!_moduleStates.TryAdd(module.GetType(), state))
        {
            // Module already exists
            return;
        }

        lock (_stateLock)
        {
            var dependencies = module.GetModuleDependencies();
            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                if (_moduleStates.TryGetValue(dependencyType, out var dependencyState))
                {
                    state.UnresolvedDependencies.Add(dependencyType);
                    dependencyState.DependentModules.Add(state);
                }
                else if (!ignoreIfNotRegistered)
                {
                    _logger.LogWarning(
                        "Dynamically added module {ModuleName} depends on {DependencyName} which is not registered",
                        MarkupFormatter.FormatModuleName(module.GetType().Name),
                        MarkupFormatter.FormatModuleName(dependencyType.Name));
                }
            }

            _logger.LogDebug(
                "Dynamically added module {ModuleName} with {DependencyCount} dependencies",
                MarkupFormatter.FormatModuleName(module.GetType().Name),
                state.UnresolvedDependencies.Count);
        }

        _schedulerNotification.Release();
    }

    /// <summary>
    /// Starts the scheduler loop that continuously queues ready modules
    /// </summary>
    public Task RunSchedulerAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                _logger.LogDebug("Module scheduler started");

                while (!cancellationToken.IsCancellationRequested && !_isDisposed)
                {
                    var queuedCount = await FindAndQueueReadyModulesAsync(cancellationToken);

                    bool allCompleted;
                    bool noPendingWork;

                    lock (_stateLock)
                    {
                        allCompleted = _moduleStates.Values.All(m => m.IsCompleted);
                        noPendingWork = _moduleStates.Values.All(m => m.IsCompleted || m.IsQueued || m.IsExecuting);
                    }

                    if (allCompleted || (noPendingWork && queuedCount == 0))
                    {
                        _logger.LogDebug("All modules scheduled, completing scheduler");
                        break;
                    }

                    // Wait for notification of module completion or new module addition
                    // Use a timeout to periodically re-check in case we miss a signal
                    try
                    {
                        await _schedulerNotification.WaitAsync(TimeSpan.FromMilliseconds(100), cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }

                _readyChannel.Writer.Complete();
                _schedulerCompleted = true;
                _logger.LogDebug("Module scheduler completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Module scheduler encountered an error");
                _readyChannel.Writer.Complete(ex);
                throw;
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Marks a module as started execution
    /// </summary>
    public void MarkModuleStarted(Type moduleType)
    {
        lock (_stateLock)
        {
            if (_moduleStates.TryGetValue(moduleType, out var state))
            {
                state.IsQueued = false;  // Clear queued flag now that it's executing
                state.IsExecuting = true;
                state.ExecutionStartTime = DateTimeOffset.UtcNow;

                if (state.QueuedTime.HasValue)
                {
                    var queueTime = state.ExecutionStartTime.Value - state.QueuedTime.Value;
                    _logger.LogDebug(
                        "Module {ModuleName} waited {QueueTime}ms in queue before execution",
                        MarkupFormatter.FormatModuleName(state.Module.GetType().Name),
                        queueTime.TotalMilliseconds);
                }
            }
        }
    }

    /// <summary>
    /// Marks a module as completed and notifies dependents
    /// </summary>
    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null)
    {
        if (!_moduleStates.TryGetValue(moduleType, out var state))
        {
            return;
        }

        lock (_stateLock)
        {
            state.IsExecuting = false;
            state.IsCompleted = true;
            state.CompletionTime = DateTimeOffset.UtcNow;

            if (success)
            {
                state.CompletionSource.TrySetResult(state.Module);
            }
            else if (exception != null)
            {
                state.CompletionSource.TrySetException(exception);
            }
            else
            {
                state.CompletionSource.TrySetCanceled();
            }

            var moduleName = MarkupFormatter.FormatModuleName(state.Module.GetType().Name);

            if (state.ExecutionStartTime.HasValue)
            {
                var executionTime = state.CompletionTime.Value - state.ExecutionStartTime.Value;
                _logger.LogDebug(
                    "Module {ModuleName} completed after {ExecutionTime}ms",
                    moduleName,
                    executionTime.TotalMilliseconds);
            }

            NotifyDependentModules(state, moduleType);
        }

        if (!_schedulerCompleted)
        {
            _schedulerNotification.Release();
        }
    }

    /// <summary>
    /// Gets the completion task for a specific module
    /// </summary>
    public Task<ModuleBase>? GetModuleCompletionTask(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state)
            ? state.CompletionSource.Task
            : null;
    }

    /// <summary>
    /// Gets the state for a specific module
    /// </summary>
    public ModuleState? GetModuleState(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state) ? state : null;
    }

    /// <summary>
    /// Gets statistics about the current scheduler state
    /// </summary>
    public (int Total, int Queued, int Executing, int Completed, int Pending) GetStatistics()
    {
        var states = _moduleStates.Values;
        return (
            Total: states.Count,
            Queued: states.Count(m => m.IsQueued && !m.IsExecuting),
            Executing: states.Count(m => m.IsExecuting),
            Completed: states.Count(m => m.IsCompleted),
            Pending: states.Count(m => !m.IsQueued && !m.IsExecuting && !m.IsCompleted)
        );
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _disposalCancellationTokenSource.Cancel();
        _schedulerNotification.Dispose();
        _disposalCancellationTokenSource.Dispose();
    }

    /// <summary>
    /// Finds modules ready to execute and queues them to the channel
    /// </summary>
    /// <returns>The number of modules queued</returns>
    private async Task<int> FindAndQueueReadyModulesAsync(CancellationToken cancellationToken)
    {
        List<ModuleState> modulesToQueue;

        // Critical section: find ready modules and mark as queued atomically
        lock (_stateLock)
        {
            var potentiallyReadyModules = _moduleStates.Values
                .Where(m => m.IsReadyToExecute)
                .ToList();

            modulesToQueue = new List<ModuleState>();

            foreach (var moduleState in potentiallyReadyModules)
            {
                // Check constraints right before queuing to ensure we see updated state
                if (!CanExecuteRespectingConstraints(moduleState))
                {
                    continue;
                }

                // Mark as queued inside lock to prevent race conditions
                moduleState.IsQueued = true;
                moduleState.QueuedTime = DateTimeOffset.UtcNow;

                modulesToQueue.Add(moduleState);
            }
        }

        // Queue to channel outside lock (async operation)
        foreach (var moduleState in modulesToQueue)
        {
            await _readyChannel.Writer.WriteAsync(moduleState, cancellationToken);

            _logger.LogDebug(
                "Queued module {ModuleName} for execution",
                MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name));
        }

        if (modulesToQueue.Count > 0)
        {
            _logger.LogDebug(
                "Scheduler found {Count} ready modules: {Modules}",
                modulesToQueue.Count,
                string.Join(", ", modulesToQueue.Select(m => MarkupFormatter.FormatModuleName(m.Module.GetType().Name))));
        }

        return modulesToQueue.Count;
    }

    /// <summary>
    /// Checks if a module can execute respecting its constraints
    /// MUST be called while holding _stateLock
    /// </summary>
    private bool CanExecuteRespectingConstraints(ModuleState moduleState)
    {
        var sequentialModuleRunning = _moduleStates.Values
            .FirstOrDefault(m =>
                m != moduleState &&
                m.RequiresSequentialExecution &&
                (m.IsExecuting || m.IsQueued));

        if (sequentialModuleRunning != null)
        {
            _logger.LogTrace(
                "Module {ModuleName} cannot execute yet (sequential module {SequentialModule} is running)",
                MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                MarkupFormatter.FormatModuleName(sequentialModuleRunning.Module.GetType().Name));
            return false;
        }

        if (moduleState.RequiresSequentialExecution)
        {
            var anyOtherExecuting = _moduleStates.Values
                .Any(m => m != moduleState && (m.IsExecuting || m.IsQueued));

            if (anyOtherExecuting)
            {
                _logger.LogTrace(
                    "Module {ModuleName} cannot execute yet (sequential constraint - other modules still running)",
                    MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name));
                return false;
            }
        }

        if (moduleState.RequiredLockKeys.Length > 0)
        {
            var conflictingModule = _moduleStates.Values
                .FirstOrDefault(m =>
                    m != moduleState &&
                    (m.IsExecuting || m.IsQueued) &&
                    m.RequiredLockKeys.Intersect(moduleState.RequiredLockKeys).Any());

            if (conflictingModule != null)
            {
                _logger.LogTrace(
                    "Module {ModuleName} cannot execute yet (lock conflict with {ConflictingModule} on keys: {Keys})",
                    MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                    MarkupFormatter.FormatModuleName(conflictingModule.Module.GetType().Name),
                    string.Join(", ", moduleState.RequiredLockKeys.Intersect(conflictingModule.RequiredLockKeys)));
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Notifies dependent modules that a dependency has completed
    /// MUST be called while holding _stateLock
    /// </summary>
    private void NotifyDependentModules(ModuleState state, Type moduleType)
    {
        if (!state.DependentModules.Any())
        {
            return;
        }

        var moduleName = MarkupFormatter.FormatModuleName(state.Module.GetType().Name);

        _logger.LogDebug(
            "Module {ModuleName} completion unblocks {Count} dependents: {Dependents}",
            moduleName,
            state.DependentModules.Count,
            string.Join(", ", state.DependentModules.Select(d => MarkupFormatter.FormatModuleName(d.Module.GetType().Name))));

        foreach (var dependent in state.DependentModules)
        {
            dependent.UnresolvedDependencies.Remove(moduleType);

            if (dependent.IsReadyToExecute)
            {
                _logger.LogDebug(
                    "Dependent module {DependentName} now ready to execute (all dependencies met)",
                    MarkupFormatter.FormatModuleName(dependent.Module.GetType().Name));
            }
        }
    }
}
