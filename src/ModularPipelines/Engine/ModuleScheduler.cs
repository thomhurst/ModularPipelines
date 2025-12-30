using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages eager parallel scheduling of modules using channels.
/// </summary>
internal class ModuleScheduler : IModuleScheduler
{
    private readonly ILogger _logger;
    private readonly TimeProvider _timeProvider;
    private readonly SchedulerOptions _options;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleConstraintEvaluator _constraintEvaluator;
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;
    private readonly HashSet<ModuleState> _queuedModules;
    private readonly HashSet<ModuleState> _executingModules;
    private readonly ModuleStateQueries _stateQueries;
    private readonly SchedulerExitConditions _exitConditions;
    private readonly Channel<ModuleState> _readyChannel;
    private readonly SemaphoreSlim _schedulerNotification;
    private readonly CancellationTokenSource _disposalCancellationTokenSource;
    private readonly ReaderWriterLockSlim _stateLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    private bool _schedulerCompleted;
    private bool _isDisposed;

    public ModuleScheduler(
        ILogger logger,
        TimeProvider timeProvider,
        IOptions<SchedulerOptions> options,
        IModuleDependencyRegistry dependencyRegistry,
        IMetricsCollector metricsCollector,
        IModuleConstraintEvaluator constraintEvaluator)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _options = options.Value;
        _dependencyRegistry = dependencyRegistry;
        _metricsCollector = metricsCollector;
        _constraintEvaluator = constraintEvaluator;
        _moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        _queuedModules = new HashSet<ModuleState>();
        _executingModules = new HashSet<ModuleState>();
        _stateQueries = new ModuleStateQueries(_moduleStates);
        _exitConditions = new SchedulerExitConditions();
        _readyChannel = Channel.CreateUnbounded<ModuleState>(new UnboundedChannelOptions
        {
            SingleWriter = true,  // Only scheduler writes
            SingleReader = false, // Multiple workers read
        });
        _schedulerNotification = new SemaphoreSlim(0);
        _disposalCancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// Gets the channel reader for consuming ready modules.
    /// </summary>
    public ChannelReader<ModuleState> ReadyModules => _readyChannel.Reader;

    /// <summary>
    /// Initializes module states for a collection of modules.
    /// </summary>
    public void InitializeModules(IEnumerable<IModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        var moduleList = modules.ToList();

        foreach (var module in moduleList)
        {
            var moduleType = module.GetType();
            var state = new ModuleState(module, moduleType);
            _moduleStates.TryAdd(moduleType, state);
        }

        // Get all available module types for DependsOnAllModulesInheritingFrom resolution
        var availableModuleTypes = _moduleStates.Keys.ToList();

        foreach (var state in _moduleStates.Values)
        {
            var moduleType = state.ModuleType;

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

            // Read priority attribute
            var priorityAttr = moduleType.GetCustomAttribute<PriorityAttribute>();
            if (priorityAttr != null)
            {
                state.Priority = priorityAttr.Priority;
                _logger.LogDebug(
                    "Module {ModuleName} has priority: {Priority}",
                    MarkupFormatter.FormatModuleName(moduleType.Name),
                    state.Priority);
            }

            // Read execution hint attribute
            var executionHintAttr = moduleType.GetCustomAttribute<ExecutionHintAttribute>();
            if (executionHintAttr != null)
            {
                state.ExecutionType = executionHintAttr.ExecutionType;
                _logger.LogDebug(
                    "Module {ModuleName} has execution type: {ExecutionType}",
                    MarkupFormatter.FormatModuleName(moduleType.Name),
                    state.ExecutionType);
            }

            // Use the overload that includes dynamic dependencies from registration events
            var dependencies = ModuleDependencyResolver.GetAllDependencies(moduleType, availableModuleTypes, _dependencyRegistry);

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
                        MarkupFormatter.FormatModuleName(state.ModuleType.Name),
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
    /// Adds a new module dynamically (e.g., SubModule discovered during execution).
    /// </summary>
    public void AddModule(IModule module)
    {
        var moduleType = module.GetType();
        var state = new ModuleState(module, moduleType);

        if (!_moduleStates.TryAdd(moduleType, state))
        {
            // Module already exists
            return;
        }

        // Track unregistered dependencies for logging outside lock
        List<Type>? unregisteredDependencies = null;

        _stateLock.EnterWriteLock();
        try
        {
            // Resolve dependencies inside write lock to prevent race conditions
            // where _moduleStates could change between resolution and processing
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType);

            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                if (_moduleStates.TryGetValue(dependencyType, out var dependencyState))
                {
                    state.UnresolvedDependencies.Add(dependencyType);
                    dependencyState.DependentModules.Add(state);
                }
                else if (!ignoreIfNotRegistered)
                {
                    unregisteredDependencies ??= new List<Type>();
                    unregisteredDependencies.Add(dependencyType);
                }
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Logging outside lock
        if (unregisteredDependencies != null)
        {
            foreach (var dependencyType in unregisteredDependencies)
            {
                _logger.LogWarning(
                    "Dynamically added module {ModuleName} depends on {DependencyName} which is not registered",
                    MarkupFormatter.FormatModuleName(moduleType.Name),
                    MarkupFormatter.FormatModuleName(dependencyType.Name));
            }
        }

        _logger.LogDebug(
            "Dynamically added module {ModuleName} with {DependencyCount} dependencies",
            MarkupFormatter.FormatModuleName(moduleType.Name),
            state.UnresolvedDependencies.Count);

        _schedulerNotification.Release();
    }

    /// <summary>
    /// Starts the scheduler loop that continuously queues ready modules.
    /// </summary>
    public Task RunSchedulerAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                _logger.LogDebug("Module scheduler started");
                await RunSchedulerLoopAsync(cancellationToken).ConfigureAwait(false);
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
            var queuedCount = await FindAndQueueReadyModulesAsync(cancellationToken).ConfigureAwait(false);

            if (ShouldExitScheduler(queuedCount))
            {
                _logger.LogDebug("All modules scheduled, completing scheduler");
                break;
            }

            if (queuedCount == 0)
            {
                LogSchedulerWaitingState();
            }

            await WaitForNextSchedulingOpportunity(cancellationToken).ConfigureAwait(false);
        }
    }

    private bool ShouldContinueScheduling(CancellationToken cancellationToken)
    {
        return !_isDisposed && !cancellationToken.IsCancellationRequested;
    }

    private bool ShouldExitScheduler(int queuedCount)
    {
        // Keep read lock held while evaluating exit conditions to prevent state changes
        ModuleStateSnapshot snapshot;
        bool shouldExit;
        bool isDeadlocked;

        _stateLock.EnterReadLock();
        try
        {
            snapshot = ModuleStateSnapshot.Create(_moduleStates.Values);
            shouldExit = _exitConditions.ShouldExit(snapshot, queuedCount);
            isDeadlocked = shouldExit && _exitConditions.IsDeadlocked(snapshot, queuedCount);

            if (shouldExit)
            {
                // Upgrade to write lock to update _schedulerCompleted
                _stateLock.ExitReadLock();
                _stateLock.EnterWriteLock();
                try
                {
                    _schedulerCompleted = true;
                }
                finally
                {
                    _stateLock.ExitWriteLock();
                }

                if (isDeadlocked)
                {
                    _logger.LogWarning(
                        "Scheduler detected deadlock: {Pending} modules pending but cannot make progress. " +
                        "Check for circular dependencies or missing module registrations.",
                        snapshot.Pending);
                }

                return true;
            }

            return false;
        }
        finally
        {
            if (_stateLock.IsReadLockHeld)
            {
                _stateLock.ExitReadLock();
            }
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
        List<ModuleState> pending;
        _stateLock.EnterReadLock();
        try
        {
            pending = _stateQueries.GetPendingModules().ToList();
        }
        finally
        {
            _stateLock.ExitReadLock();
        }

        // Log outside lock
        if (pending.Count > 0)
        {
            _logger.LogDebug("Pending modules: {Modules}",
                string.Join(", ", pending.Select(FormatModuleWithDependencyCount)));
        }
    }

    private void LogExecutingModules()
    {
        List<ModuleState> executing;
        _stateLock.EnterReadLock();
        try
        {
            executing = _stateQueries.GetExecutingModules().ToList();
        }
        finally
        {
            _stateLock.ExitReadLock();
        }

        // Log outside lock
        if (executing.Count > 0)
        {
            _logger.LogDebug("Executing modules: {Modules}",
                string.Join(", ", executing.Select(m => MarkupFormatter.FormatModuleName(m.ModuleType.Name))));
        }
    }

    private string FormatModuleWithDependencyCount(ModuleState m)
    {
        return $"{MarkupFormatter.FormatModuleName(m.ModuleType.Name)} (deps: {m.UnresolvedDependencies.Count})";
    }

    private async Task WaitForNextSchedulingOpportunity(CancellationToken cancellationToken)
    {
        try
        {
            await _schedulerNotification.WaitAsync(_options.NotificationTimeout, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _stateLock.EnterWriteLock();
            try
            {
                _schedulerCompleted = true;
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }

            throw;
        }
    }

    private void CompleteScheduler()
    {
        _stateLock.EnterWriteLock();
        try
        {
            if (!_schedulerCompleted)
            {
                _schedulerCompleted = true;
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        _readyChannel.Writer.Complete();
    }

    /// <summary>
    /// Marks a module as started execution.
    /// </summary>
    /// <returns>True if the module can proceed with execution, false if constraints prevent execution.</returns>
    public bool MarkModuleStarted(Type moduleType)
    {
        _stateLock.EnterWriteLock();
        try
        {
            if (_moduleStates.TryGetValue(moduleType, out var state))
            {
                // Delegate constraint checking to the evaluator
                if (!_constraintEvaluator.CanStartExecution(state, _executingModules))
                {
                    // Reset to Pending so scheduler will retry when constraints allow
                    _queuedModules.Remove(state);
                    state.State = ModuleExecutionState.Pending;
                    state.QueuedTime = null;

                    // Notify the scheduler to reschedule
                    _schedulerNotification.Release();
                    return false;
                }

                _queuedModules.Remove(state);
                state.State = ModuleExecutionState.Executing;
                _executingModules.Add(state);
                state.ExecutionStartTime = _timeProvider.GetUtcNow();

                // Record metrics
                _metricsCollector.RecordModuleStarted(moduleType, state.ExecutionStartTime.Value);
                _metricsCollector.RecordConcurrencySnapshot(_executingModules.Count, state.ExecutionStartTime.Value);

                return true;
            }

            return false;
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Marks a module as completed and notifies dependents.
    /// </summary>
    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null)
    {
        if (!_moduleStates.TryGetValue(moduleType, out var state))
        {
            return;
        }

        // Capture values for logging outside lock
        int queuedCount;
        int executingCount;
        bool shouldNotify;

        _stateLock.EnterWriteLock();
        try
        {
            _executingModules.Remove(state);
            state.State = ModuleExecutionState.Completed;
            state.CompletionTime = _timeProvider.GetUtcNow();

            // Record metrics
            var wasSkipped = state.SkipResult != null && state.SkipResult != Models.SkipDecision.DoNotSkip;
            var status = wasSkipped ? Enums.Status.Skipped : (success ? Enums.Status.Successful : Enums.Status.Failed);
            _metricsCollector.RecordModuleCompleted(moduleType, state.CompletionTime.Value, success, wasSkipped, status);
            _metricsCollector.RecordConcurrencySnapshot(_executingModules.Count, state.CompletionTime.Value);

            // Capture counts for logging outside lock
            queuedCount = _queuedModules.Count;
            executingCount = _executingModules.Count;

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

            NotifyDependentModules(state, moduleType);

            shouldNotify = !_schedulerCompleted;
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Logging outside lock
        _logger.LogDebug(
            "Module {ModuleName} completed with lock keys: [{Keys}] (Active: Q={Queued}, E={Executing})",
            MarkupFormatter.FormatModuleName(state.ModuleType.Name),
            string.Join(", ", state.RequiredLockKeys),
            queuedCount,
            executingCount);

        var moduleName = MarkupFormatter.FormatModuleName(state.ModuleType.Name);

        if (state.ExecutionStartTime.HasValue && state.CompletionTime.HasValue)
        {
            var executionTime = state.CompletionTime.Value - state.ExecutionStartTime.Value;
            _logger.LogDebug(
                "Module {ModuleName} completed after {ExecutionTime}ms",
                moduleName,
                executionTime.TotalMilliseconds);
        }

        if (shouldNotify)
        {
            _schedulerNotification.Release();
        }
    }

    /// <summary>
    /// Gets the completion task for a specific module.
    /// </summary>
    public Task<IModule>? GetModuleCompletionTask(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state)
            ? state.CompletionSource.Task
            : null;
    }

    /// <summary>
    /// Gets the state for a specific module.
    /// </summary>
    public ModuleState? GetModuleState(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state) ? state : null;
    }

    /// <summary>
    /// Gets statistics about the current scheduler state.
    /// </summary>
    public (int Total, int Queued, int Executing, int Completed, int Pending) GetStatistics()
    {
        _stateLock.EnterReadLock();
        try
        {
            var stats = _stateQueries.GetStatistics();
            return (stats.Total, stats.Queued, stats.Executing, stats.Completed, stats.Pending);
        }
        finally
        {
            _stateLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Cancels all modules that are queued or pending (not yet executing)
    /// This is used when the pipeline is cancelled to ensure TaskCompletionSources are properly completed
    /// Note: AlwaysRun modules are not cancelled as they should be allowed to complete.
    /// </summary>
    public void CancelPendingModules()
    {
        List<(ModuleState Module, ModuleExecutionState OriginalState)> cancelledModules;

        _stateLock.EnterWriteLock();
        try
        {
            var pendingModules = _stateQueries.GetCancellablePendingModules().ToList();
            cancelledModules = new List<(ModuleState, ModuleExecutionState)>();

            foreach (var moduleState in pendingModules)
            {
                if (moduleState.State != ModuleExecutionState.Completed)
                {
                    var originalState = moduleState.State;
                    _queuedModules.Remove(moduleState);
                    _executingModules.Remove(moduleState);
                    moduleState.State = ModuleExecutionState.Completed;
                    moduleState.CompletionSource.TrySetCanceled();
                    cancelledModules.Add((moduleState, originalState));
                }
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Logging outside lock
        _logger.LogDebug("Cancelling {Count} pending/queued modules due to pipeline cancellation (excluding AlwaysRun modules)", cancelledModules.Count);

        foreach (var (moduleState, originalState) in cancelledModules)
        {
            _logger.LogDebug(
                "Cancelling pending module {ModuleName} (State={State})",
                MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                originalState);
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
        _stateLock.Dispose();
    }

    /// <summary>
    /// Finds modules ready to execute and queues them to the channel.
    /// </summary>
    /// <returns>The number of modules queued.</returns>
    private async Task<int> FindAndQueueReadyModulesAsync(CancellationToken cancellationToken)
    {
        var modulesToQueue = FindReadyModules();

        if (modulesToQueue.Count > 0)
        {
            await QueueModulesForExecutionAsync(modulesToQueue, cancellationToken).ConfigureAwait(false);
            LogQueuedModules(modulesToQueue);
        }

        return modulesToQueue.Count;
    }

    private List<ModuleState> FindReadyModules()
    {
        _stateLock.EnterWriteLock();
        try
        {
            // Sort by priority descending so higher priority modules are queued first
            var potentiallyReadyModules = _moduleStates.Values
                .Where(m => m.IsReadyToExecute)
                .OrderByDescending(m => (int)m.Priority)
                .ToList();

            var modulesToQueue = new List<ModuleState>();

            foreach (var moduleState in potentiallyReadyModules)
            {
                if (!CanExecuteRespectingConstraints(moduleState))
                {
                    continue;
                }

                // Set ReadyTime if not already set (for modules with no dependencies)
                var now = _timeProvider.GetUtcNow();
                moduleState.ReadyTime ??= now;

                // Record metrics
                _metricsCollector.RecordModuleReady(moduleState.ModuleType, moduleState.ReadyTime.Value, moduleState.Priority, moduleState.ExecutionType);
                _metricsCollector.RecordModuleQueued(moduleState.ModuleType, now);

                moduleState.State = ModuleExecutionState.Queued;
                moduleState.QueuedTime = now;
                _queuedModules.Add(moduleState);

                modulesToQueue.Add(moduleState);
            }

            return modulesToQueue;
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }
    }

    private async Task QueueModulesForExecutionAsync(List<ModuleState> modulesToQueue, CancellationToken cancellationToken)
    {
        // Queue to channel outside lock (async operation)
        foreach (var moduleState in modulesToQueue)
        {
            await _readyChannel.Writer.WriteAsync(moduleState, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug(
                "Queued module {ModuleName} for execution",
                MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name));
        }
    }

    private void LogQueuedModules(List<ModuleState> modulesToQueue)
    {
        _logger.LogDebug(
            "Scheduler found {Count} ready modules: {Modules}",
            modulesToQueue.Count,
            string.Join(", ", modulesToQueue.Select(m => MarkupFormatter.FormatModuleName(m.ModuleType.Name))));
    }

    /// <summary>
    /// Checks if a module can execute respecting its constraints.
    /// MUST be called while holding _stateLock.
    /// </summary>
    private bool CanExecuteRespectingConstraints(ModuleState moduleState)
    {
        // Check against modules with Queued or Executing state (source of truth)
        // Using State enum instead of tracking collections prevents race conditions
        var activeModules = _moduleStates.Values
            .Where(m => m.State == ModuleExecutionState.Queued || m.State == ModuleExecutionState.Executing);

        // Delegate constraint checking to the evaluator
        return _constraintEvaluator.CanQueue(moduleState, activeModules);
    }

    /// <summary>
    /// Notifies dependent modules that a dependency has completed
    /// MUST be called while holding _stateLock.
    /// </summary>
    private void NotifyDependentModules(ModuleState state, Type moduleType)
    {
        if (!state.DependentModules.Any())
        {
            return;
        }

        var moduleName = MarkupFormatter.FormatModuleName(state.ModuleType.Name);

        _logger.LogDebug(
            "Module {ModuleName} completion unblocks {Count} dependents: {Dependents}",
            moduleName,
            state.DependentModules.Count,
            string.Join(", ", state.DependentModules.Select(d => MarkupFormatter.FormatModuleName(d.ModuleType.Name))));

        foreach (var dependent in state.DependentModules)
        {
            dependent.UnresolvedDependencies.Remove(moduleType);

            if (dependent.IsReadyToExecute)
            {
                dependent.ReadyTime = _timeProvider.GetUtcNow();
                _logger.LogDebug(
                    "Dependent module {DependentName} now ready to execute (all dependencies met)",
                    MarkupFormatter.FormatModuleName(dependent.ModuleType.Name));
            }
        }
    }
}
