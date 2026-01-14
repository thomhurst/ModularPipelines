using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages module state transitions during pipeline execution.
/// </summary>
/// <remarks>
/// This class encapsulates the state management responsibility previously
/// embedded in ModuleScheduler, following the Single Responsibility Principle.
///
/// Thread Safety: All state transitions are protected by the provided ReaderWriterLockSlim.
/// The lock is owned by the caller (ModuleScheduler) to allow coordination with other operations.
/// </remarks>
internal class ModuleStateTracker : IModuleStateTracker
{
    private readonly ILogger _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleConstraintEvaluator _constraintEvaluator;
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;
    private readonly HashSet<ModuleState> _queuedModules;
    private readonly HashSet<ModuleState> _executingModules;
    private readonly ModuleStateQueries _stateQueries;
    private readonly ReaderWriterLockSlim _stateLock;
    private readonly SemaphoreSlim _schedulerNotification;
    private readonly Func<bool> _isSchedulerCompleted;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleStateTracker"/> class.
    /// </summary>
    /// <param name="logger">Logger for diagnostic output.</param>
    /// <param name="timeProvider">Provider for current time.</param>
    /// <param name="metricsCollector">Collector for execution metrics.</param>
    /// <param name="constraintEvaluator">Evaluator for module execution constraints.</param>
    /// <param name="moduleStates">Shared dictionary of module states.</param>
    /// <param name="queuedModules">Set of modules currently in the queue.</param>
    /// <param name="executingModules">Set of modules currently executing.</param>
    /// <param name="stateQueries">Query helper for module states.</param>
    /// <param name="stateLock">Shared lock for state access synchronization.</param>
    /// <param name="schedulerNotification">Semaphore to notify scheduler of state changes.</param>
    /// <param name="isSchedulerCompleted">Function to check if scheduler has completed.</param>
    public ModuleStateTracker(
        ILogger logger,
        TimeProvider timeProvider,
        IMetricsCollector metricsCollector,
        IModuleConstraintEvaluator constraintEvaluator,
        ConcurrentDictionary<Type, ModuleState> moduleStates,
        HashSet<ModuleState> queuedModules,
        HashSet<ModuleState> executingModules,
        ModuleStateQueries stateQueries,
        ReaderWriterLockSlim stateLock,
        SemaphoreSlim schedulerNotification,
        Func<bool> isSchedulerCompleted)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _metricsCollector = metricsCollector;
        _constraintEvaluator = constraintEvaluator;
        _moduleStates = moduleStates;
        _queuedModules = queuedModules;
        _executingModules = executingModules;
        _stateQueries = stateQueries;
        _stateLock = stateLock;
        _schedulerNotification = schedulerNotification;
        _isSchedulerCompleted = isSchedulerCompleted;
    }

    /// <inheritdoc />
    public bool MarkModuleStarted(Type moduleType)
    {
        // Use copy-on-read pattern: collect data inside lock, process outside
        // This prevents LockRecursionException if constraint evaluator or metrics
        // collector callbacks try to access scheduler state
        //
        // IMPORTANT: Constraint checking uses a SNAPSHOT of executing modules,
        // so we can safely call CanStartExecution inside the lock without risk
        // of lock recursion (the snapshot is a plain List, not protected by lock).
        // Releasing the lock between constraint check and state mutation would
        // create a race condition where another thread could start a conflicting module.
        bool needsReschedule = false;
        DateTimeOffset? executionStartTime = null;
        int executingCount = 0;
        bool result = false;

        _stateLock.EnterWriteLock();
        try
        {
            if (!_moduleStates.TryGetValue(moduleType, out var state))
            {
                return false;
            }

            // Take snapshot of executing modules for constraint checking
            // The constraint evaluator operates on this snapshot, not the live collection
            var executingSnapshot = _executingModules.ToList();

            // Check constraints - this is safe because CanStartExecution works on the snapshot
            // and should NOT access scheduler state directly
            if (!_constraintEvaluator.CanStartExecution(state, executingSnapshot))
            {
                // Reset to Pending so scheduler will retry when constraints allow
                _queuedModules.Remove(state);
                state.State = ModuleExecutionState.Pending;
                state.QueuedTime = null;
                needsReschedule = true;
                return false;
            }

            _queuedModules.Remove(state);
            state.State = ModuleExecutionState.Executing;
            _executingModules.Add(state);
            state.ExecutionStartTime = _timeProvider.GetUtcNow();

            // Capture data for metrics recording outside lock
            executionStartTime = state.ExecutionStartTime;
            executingCount = _executingModules.Count;
            result = true;
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Notify scheduler outside lock if needed
        if (needsReschedule)
        {
            _schedulerNotification.Release();
        }

        // Record metrics outside lock to prevent lock recursion
        if (executionStartTime.HasValue)
        {
            _metricsCollector.RecordModuleStarted(moduleType, executionStartTime.Value);
            _metricsCollector.RecordConcurrencySnapshot(executingCount, executionStartTime.Value);
        }

        return result;
    }

    /// <inheritdoc />
    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null)
    {
        if (!_moduleStates.TryGetValue(moduleType, out var state))
        {
            return;
        }

        // Use copy-on-read pattern: collect data inside lock, process outside
        // This prevents LockRecursionException if metrics collector callbacks
        // try to access scheduler state
        int queuedCount;
        int executingCount;
        bool shouldNotify;
        DateTimeOffset completionTime;
        bool wasSkipped;
        Enums.Status status;
        List<(ModuleState Dependent, bool IsReady)> dependentUpdates;

        _stateLock.EnterWriteLock();
        try
        {
            _executingModules.Remove(state);
            state.State = ModuleExecutionState.Completed;
            state.CompletionTime = _timeProvider.GetUtcNow();

            // Capture data for metrics recording outside lock
            completionTime = state.CompletionTime.Value;
            wasSkipped = state.SkipResult != null && state.SkipResult != Models.SkipDecision.DoNotSkip;
            status = wasSkipped ? Enums.Status.Skipped : (success ? Enums.Status.Successful : Enums.Status.Failed);

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

            // Process dependent modules and capture updates for logging outside lock
            dependentUpdates = ProcessDependentModules(state, moduleType);

            shouldNotify = !_isSchedulerCompleted();
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Record metrics outside lock to prevent lock recursion
        _metricsCollector.RecordModuleCompleted(moduleType, completionTime, success, wasSkipped, status);
        _metricsCollector.RecordConcurrencySnapshot(executingCount, completionTime);

        // Logging outside lock - use plain text, not markup (markup causes double-escaping issues)
        _logger.LogDebug(
            "Module {ModuleName} completed with lock keys: {Keys} (Active: Q={Queued}, E={Executing})",
            state.ModuleType.Name,
            string.Join(", ", state.RequiredLockKeys),
            queuedCount,
            executingCount);

        var moduleName = state.ModuleType.Name;

        if (state.ExecutionStartTime.HasValue && state.CompletionTime.HasValue)
        {
            var executionTime = state.CompletionTime.Value - state.ExecutionStartTime.Value;
            _logger.LogDebug(
                "Module {ModuleName} completed after {ExecutionTime}ms",
                moduleName,
                executionTime.TotalMilliseconds);
        }

        // Log dependent module updates outside lock
        LogDependentModuleUpdates(state, dependentUpdates);

        if (shouldNotify)
        {
            _schedulerNotification.Release();
        }
    }

    /// <inheritdoc />
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
                moduleState.ModuleType.Name,
                originalState);
        }
    }

    /// <inheritdoc />
    public ModuleState? GetModuleState(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state) ? state : null;
    }

    /// <inheritdoc />
    public Task<IModule>? GetModuleCompletionTask(Type moduleType)
    {
        return _moduleStates.TryGetValue(moduleType, out var state)
            ? state.CompletionSource.Task
            : null;
    }

    /// <summary>
    /// Processes dependent modules and captures state for logging outside lock.
    /// MUST be called while holding _stateLock.
    /// </summary>
    /// <returns>List of dependent modules with their ready state for logging outside lock.</returns>
    private List<(ModuleState Dependent, bool IsReady)> ProcessDependentModules(ModuleState state, Type moduleType)
    {
        var updates = new List<(ModuleState, bool)>();

        foreach (var dependent in state.DependentModules)
        {
            dependent.UnresolvedDependencies.Remove(moduleType);

            if (dependent.IsReadyToExecute)
            {
                dependent.ReadyTime = _timeProvider.GetUtcNow();
                updates.Add((dependent, true));
            }
            else
            {
                updates.Add((dependent, false));
            }
        }

        return updates;
    }

    /// <summary>
    /// Logs dependent module updates. Called outside the lock.
    /// </summary>
    private void LogDependentModuleUpdates(ModuleState state, List<(ModuleState Dependent, bool IsReady)> updates)
    {
        if (updates.Count == 0)
        {
            return;
        }

        _logger.LogDebug(
            "Module {ModuleName} completion unblocks {Count} dependents: {Dependents}",
            state.ModuleType.Name,
            updates.Count,
            string.Join(", ", updates.Select(u => u.Dependent.ModuleType.Name)));

        foreach (var (dependent, isReady) in updates)
        {
            if (isReady)
            {
                _logger.LogDebug(
                    "Dependent module {DependentName} now ready to execute (all dependencies met)",
                    dependent.ModuleType.Name);
            }
        }
    }
}
