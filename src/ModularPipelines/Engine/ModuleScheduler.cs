using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Engine.Constraints;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages eager parallel scheduling of modules using channels
/// </summary>
internal class ModuleScheduler : IModuleScheduler
{
    private readonly ILogger _logger;
    private readonly TimeProvider _timeProvider;
    private readonly SchedulerOptions _options;
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;
    private readonly HashSet<ModuleState> _queuedModules;
    private readonly HashSet<ModuleState> _executingModules;
    private readonly ModuleStateQueries _stateQueries;
    private readonly SchedulerExitConditions _exitConditions;
    private readonly IModuleConstraint[] _constraints;
    private readonly Channel<ModuleState> _readyChannel;
    private readonly SemaphoreSlim _schedulerNotification;
    private readonly CancellationTokenSource _disposalCancellationTokenSource;
    private readonly object _stateLock = new object();

    private bool _schedulerCompleted;
    private bool _isDisposed;

    public ModuleScheduler(ILogger logger, TimeProvider timeProvider, IOptions<SchedulerOptions> options)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _options = options.Value;
        _moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        _queuedModules = new HashSet<ModuleState>();
        _executingModules = new HashSet<ModuleState>();
        _stateQueries = new ModuleStateQueries(_moduleStates);
        _exitConditions = new SchedulerExitConditions();
        _constraints = new IModuleConstraint[]
        {
            new SequentialExecutionConstraint(),
            new LockKeyConstraint()
        };
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
    public void InitializeModules(IEnumerable<IModule> modules)
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
    public void AddModule(IModule module)
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
                await RunSchedulerLoopAsync(cancellationToken);
                CompleteScheduler();
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

    private async Task RunSchedulerLoopAsync(CancellationToken cancellationToken)
    {
        while (ShouldContinueScheduling(cancellationToken))
        {
            var queuedCount = await FindAndQueueReadyModulesAsync(cancellationToken);

            if (ShouldExitScheduler(queuedCount))
            {
                _logger.LogDebug("All modules scheduled, completing scheduler");
                break;
            }

            if (queuedCount == 0)
            {
                LogSchedulerWaitingState();
            }

            await WaitForNextSchedulingOpportunity(cancellationToken);
        }
    }

    private bool ShouldContinueScheduling(CancellationToken cancellationToken)
    {
        return !_isDisposed && !cancellationToken.IsCancellationRequested;
    }

    private bool ShouldExitScheduler(int queuedCount)
    {
        lock (_stateLock)
        {
            var snapshot = ModuleStateSnapshot.Create(_moduleStates.Values);
            var shouldExit = _exitConditions.ShouldExit(snapshot, queuedCount);

            if (shouldExit)
            {
                _schedulerCompleted = true;

                if (_exitConditions.IsDeadlocked(snapshot, queuedCount))
                {
                    _logger.LogWarning(
                        "Scheduler detected deadlock: {Pending} modules pending but cannot make progress. " +
                        "Check for circular dependencies or missing module registrations.",
                        snapshot.Pending);
                }
            }

            return shouldExit;
        }
    }

    private void LogSchedulerWaitingState()
    {
        var stats = _stateQueries.GetStatistics();
        _logger.LogDebug(
            "Scheduler waiting: Total={Total}, Queued={Queued}, Executing={Executing}, Completed={Completed}, Pending={Pending}",
            stats.Total, stats.Queued, stats.Executing, stats.Completed, stats.Pending);

        LogPendingModules();
        LogExecutingModules();
    }

    private void LogPendingModules()
    {
        lock (_stateLock)
        {
            var pending = _stateQueries.GetPendingModules().ToList();
            if (pending.Any())
            {
                _logger.LogDebug("Pending modules: {Modules}",
                    string.Join(", ", pending.Select(FormatModuleWithDependencyCount)));
            }
        }
    }

    private void LogExecutingModules()
    {
        lock (_stateLock)
        {
            var executing = _stateQueries.GetExecutingModules().ToList();
            if (executing.Any())
            {
                _logger.LogDebug("Executing modules: {Modules}",
                    string.Join(", ", executing.Select(m => MarkupFormatter.FormatModuleName(m.Module.GetType().Name))));
            }
        }
    }

    private string FormatModuleWithDependencyCount(ModuleState m)
    {
        return $"{MarkupFormatter.FormatModuleName(m.Module.GetType().Name)} (deps: {m.UnresolvedDependencies.Count})";
    }

    private async Task WaitForNextSchedulingOpportunity(CancellationToken cancellationToken)
    {
        try
        {
            await _schedulerNotification.WaitAsync(_options.NotificationTimeout, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            lock (_stateLock)
            {
                _schedulerCompleted = true;
            }
            throw;
        }
    }

    private void CompleteScheduler()
    {
        lock (_stateLock)
        {
            if (!_schedulerCompleted)
            {
                _schedulerCompleted = true;
            }
        }

        _readyChannel.Writer.Complete();
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
                // Defensive constraint check at execution boundary
                // Prevents race where module was queued but constraints changed before execution
                if (!CanExecuteRespectingConstraints(state))
                {
                    _logger.LogDebug(
                        "Module {ModuleName} constraint check failed at execution time, returning to Pending for retry",
                        MarkupFormatter.FormatModuleName(state.Module.GetType().Name));

                    // Reset to Pending so scheduler will retry when constraints allow
                    _queuedModules.Remove(state);
                    state.State = ModuleExecutionState.Pending;
                    state.QueuedTime = null;
                    return;
                }

                _queuedModules.Remove(state);
                state.State = ModuleExecutionState.Executing;
                _executingModules.Add(state);
                state.ExecutionStartTime = _timeProvider.GetUtcNow();

                _logger.LogDebug(
                    "Module {ModuleName} started executing with lock keys: [{Keys}] (Active: Q={Queued}, E={Executing})",
                    MarkupFormatter.FormatModuleName(state.Module.GetType().Name),
                    string.Join(", ", state.RequiredLockKeys),
                    _queuedModules.Count,
                    _executingModules.Count);

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
            _executingModules.Remove(state);
            state.State = ModuleExecutionState.Completed;
            state.CompletionTime = _timeProvider.GetUtcNow();

            _logger.LogDebug(
                "Module {ModuleName} completed with lock keys: [{Keys}] (Active: Q={Queued}, E={Executing})",
                MarkupFormatter.FormatModuleName(state.Module.GetType().Name),
                string.Join(", ", state.RequiredLockKeys),
                _queuedModules.Count,
                _executingModules.Count);

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

            if (!_schedulerCompleted)
            {
                _schedulerNotification.Release();
            }
        }
    }

    /// <summary>
    /// Gets the completion task for a specific module
    /// </summary>
    public Task<IModule>? GetModuleCompletionTask(Type moduleType)
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
        lock (_stateLock)
        {
            var stats = _stateQueries.GetStatistics();
            return (stats.Total, stats.Queued, stats.Executing, stats.Completed, stats.Pending);
        }
    }

    /// <summary>
    /// Cancels all modules that are queued or pending (not yet executing)
    /// This is used when the pipeline is cancelled to ensure TaskCompletionSources are properly completed
    /// Note: AlwaysRun modules are not cancelled as they should be allowed to complete
    /// </summary>
    public void CancelPendingModules()
    {
        lock (_stateLock)
        {
            var pendingModules = _stateQueries.GetCancellablePendingModules().ToList();

            _logger.LogDebug("Cancelling {Count} pending/queued modules due to pipeline cancellation (excluding AlwaysRun modules)", pendingModules.Count);

            foreach (var moduleState in pendingModules)
            {
                if (moduleState.State != ModuleExecutionState.Completed)
                {
                    _logger.LogDebug(
                        "Cancelling pending module {ModuleName} (State={State})",
                        MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                        moduleState.State);

                    _queuedModules.Remove(moduleState);
                    _executingModules.Remove(moduleState);
                    moduleState.State = ModuleExecutionState.Completed;

                    if (moduleState.Module is IModuleInternal moduleInternal)
                    {
                        moduleInternal.Status = Status.PipelineTerminated;
                    }

                    moduleState.CompletionSource.TrySetCanceled();
                }
            }
        }
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
        var modulesToQueue = FindReadyModules();

        if (modulesToQueue.Count > 0)
        {
            await QueueModulesForExecutionAsync(modulesToQueue, cancellationToken);
            LogQueuedModules(modulesToQueue);
        }

        return modulesToQueue.Count;
    }

    private List<ModuleState> FindReadyModules()
    {
        lock (_stateLock)
        {
            var potentiallyReadyModules = _moduleStates.Values
                .Where(m => m.IsReadyToExecute)
                .ToList();

            var modulesToQueue = new List<ModuleState>();

            foreach (var moduleState in potentiallyReadyModules)
            {
                if (!CanExecuteRespectingConstraints(moduleState))
                {
                    _logger.LogDebug(
                        "Module {ModuleName} blocked by constraints (Active: Q={Queued}, E={Executing})",
                        MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                        _queuedModules.Count,
                        _executingModules.Count);
                    continue;
                }

                moduleState.State = ModuleExecutionState.Queued;
                moduleState.QueuedTime = _timeProvider.GetUtcNow();
                _queuedModules.Add(moduleState);

                _logger.LogDebug(
                    "Module {ModuleName} queued with lock keys: [{Keys}] (Active after: Q={Queued}, E={Executing})",
                    MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                    string.Join(", ", moduleState.RequiredLockKeys),
                    _queuedModules.Count,
                    _executingModules.Count);

                modulesToQueue.Add(moduleState);
            }

            return modulesToQueue;
        }
    }

    private async Task QueueModulesForExecutionAsync(List<ModuleState> modulesToQueue, CancellationToken cancellationToken)
    {
        // Queue to channel outside lock (async operation)
        foreach (var moduleState in modulesToQueue)
        {
            await _readyChannel.Writer.WriteAsync(moduleState, cancellationToken);

            _logger.LogDebug(
                "Queued module {ModuleName} for execution",
                MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name));
        }
    }

    private void LogQueuedModules(List<ModuleState> modulesToQueue)
    {
        _logger.LogDebug(
            "Scheduler found {Count} ready modules: {Modules}",
            modulesToQueue.Count,
            string.Join(", ", modulesToQueue.Select(m => MarkupFormatter.FormatModuleName(m.Module.GetType().Name))));
    }

    /// <summary>
    /// Checks if a module can execute respecting its constraints
    /// MUST be called while holding _stateLock
    /// </summary>
    private bool CanExecuteRespectingConstraints(ModuleState moduleState)
    {
        // Check against modules with Queued or Executing state (source of truth)
        // Using State enum instead of tracking collections prevents race conditions
        var activeModules = _moduleStates.Values
            .Where(m => m.State == ModuleExecutionState.Queued || m.State == ModuleExecutionState.Executing)
            .ToList();

        _logger.LogDebug(
            "[CONSTRAINT] Checking {ModuleName} with keys [{Keys}] against {ActiveCount} active modules",
            MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
            string.Join(", ", moduleState.RequiredLockKeys),
            activeModules.Count);

        // Check lock key conflicts
        foreach (var active in activeModules)
        {
            // Skip checking against self
            if (active == moduleState)
            {
                continue;
            }

            _logger.LogDebug(
                "[CONSTRAINT]   Comparing with active {ActiveModule} (State={State}, Keys=[{ActiveKeys}])",
                MarkupFormatter.FormatModuleName(active.Module.GetType().Name),
                active.State,
                string.Join(", ", active.RequiredLockKeys));

            // Skip if neither module has lock keys
            if (moduleState.RequiredLockKeys.Length == 0 && active.RequiredLockKeys.Length == 0)
            {
                continue;
            }

            // Check for lock key intersection
            if (moduleState.RequiredLockKeys.Length > 0 && active.RequiredLockKeys.Length > 0)
            {
                var intersection = active.RequiredLockKeys.Intersect(moduleState.RequiredLockKeys).ToArray();

                _logger.LogDebug(
                    "[CONSTRAINT]   Intersection check: Module=[{ModuleKeys}] vs Active=[{ActiveKeys}] => Intersection=[{Intersection}]",
                    string.Join(", ", moduleState.RequiredLockKeys),
                    string.Join(", ", active.RequiredLockKeys),
                    string.Join(", ", intersection));

                if (intersection.Length > 0)
                {
                    _logger.LogDebug(
                        "[CONSTRAINT] ❌ Module {ModuleName} BLOCKED by {ActiveModule} on keys: [{Keys}]",
                        MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                        MarkupFormatter.FormatModuleName(active.Module.GetType().Name),
                        string.Join(", ", intersection));
                    return false;
                }
            }
        }

        if (moduleState.RequiresSequentialExecution)
        {
            // Exclude self from count
            var otherActiveModules = activeModules.Where(m => m != moduleState).ToList();
            if (otherActiveModules.Count > 0)
            {
                _logger.LogDebug(
                    "[CONSTRAINT] Sequential module {ModuleName} blocked - {Count} other modules active",
                    MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                    otherActiveModules.Count);
                return false;
            }
        }

        foreach (var active in activeModules)
        {
            // Skip checking against self
            if (active == moduleState)
            {
                continue;
            }

            if (active.RequiresSequentialExecution)
            {
                _logger.LogDebug(
                    "[CONSTRAINT] Module {ModuleName} blocked by sequential module {SequentialModule}",
                    MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                    MarkupFormatter.FormatModuleName(active.Module.GetType().Name));
                return false;
            }
        }

        _logger.LogDebug(
            "[CONSTRAINT] ✅ Module {ModuleName} passed all constraints",
            MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name));
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
