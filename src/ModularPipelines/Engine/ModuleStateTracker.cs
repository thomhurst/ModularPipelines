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

    /// <inheritdoc />
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

            shouldNotify = !_isSchedulerCompleted();
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
                MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
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
    /// Notifies dependent modules that a dependency has completed.
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
