using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
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
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleConstraintEvaluator _constraintEvaluator;
    private readonly ISchedulerStatusReporter _statusReporter;
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;
    private readonly HashSet<ModuleState> _pendingReadyModules;
    private readonly HashSet<ModuleState> _queuedModules;
    private readonly HashSet<ModuleState> _executingModules;
    private readonly ModuleStateQueries _stateQueries;
    private readonly ModuleStateCounters _stateCounters;
    private readonly SchedulerExitConditions _exitConditions;
    private readonly Channel<ModuleState> _readyChannel;
    private readonly SemaphoreSlim _schedulerNotification;
    private readonly ReaderWriterLockSlim _stateLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
    private readonly IModuleStateTracker _stateTracker;

    private bool _hasSchedulingConstraints;
    private bool _schedulerCompleted;
    private int _disposeState;

    private bool IsDisposed => Volatile.Read(ref _disposeState) != 0;

    public ModuleScheduler(
        ILogger logger,
        TimeProvider timeProvider,
        IOptions<SchedulerOptions> options,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IMetricsCollector metricsCollector,
        IModuleConstraintEvaluator constraintEvaluator,
        ISchedulerStatusReporter statusReporter)
    {
        _logger = logger;
        _timeProvider = timeProvider;
        _options = options.Value;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _metricsCollector = metricsCollector;
        _constraintEvaluator = constraintEvaluator;
        _statusReporter = statusReporter;
        _moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        _pendingReadyModules = new HashSet<ModuleState>();
        _queuedModules = new HashSet<ModuleState>();
        _executingModules = new HashSet<ModuleState>();
        _stateQueries = new ModuleStateQueries(_moduleStates);
        _stateCounters = new ModuleStateCounters();
        _exitConditions = new SchedulerExitConditions();
        _readyChannel = Channel.CreateUnbounded<ModuleState>(new UnboundedChannelOptions
        {
            SingleWriter = true,  // Only scheduler writes
            SingleReader = false, // Multiple workers read
        });
        _schedulerNotification = new SemaphoreSlim(0);

        // Initialize state tracker with shared state
        _stateTracker = new ModuleStateTracker(
            logger,
            timeProvider,
            metricsCollector,
            constraintEvaluator,
            _moduleStates,
            _queuedModules,
            _executingModules,
            _pendingReadyModules,
            _stateQueries,
            _stateLock,
            _schedulerNotification,
            () => _schedulerCompleted,
            _stateCounters);
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

        if (IsDisposed)
        {
            return;
        }

        var moduleArray = modules.ToArray();
        AddModuleStates(moduleArray);
        var availableModuleTypes = _moduleStates.Keys.ToArray();
        FinalizeModuleMetadata();

        foreach (var state in _moduleStates.Values)
        {
            ConfigureScheduling(state);
            _metricsCollector.RecordModuleInitialized(
                state.ModuleType,
                state.Priority,
                state.ExecutionHint);
        }

        foreach (var state in _moduleStates.Values)
        {
            ConfigureDependencies(state, availableModuleTypes);
            if (state.IsReadyToExecute)
            {
                _pendingReadyModules.Add(state);
            }
            else
            {
                _pendingReadyModules.Remove(state);
            }
        }

        _logger.LogDebug(
            "Initialized {Count} modules for scheduling with total of {DependencyCount} dependencies",
            _moduleStates.Count,
            _moduleStates.Values.Sum(x => x.UnresolvedDependencies.Count));
    }

    /// <summary>
    /// Starts the scheduler loop that continuously queues ready modules.
    /// </summary>
    public Task RunSchedulerAsync(CancellationToken cancellationToken)
    {
        if (IsDisposed)
        {
            return Task.CompletedTask;
        }

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
                // Catch ALL exceptions including fatal ones - we need to complete the channel
                // and log before re-throwing. The immediate throw ensures exceptions propagate.
                _logger.LogError(ex, "Module scheduler encountered an error");
                _readyChannel.Writer.Complete(ex);
                throw;
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Marks a module as started execution.
    /// </summary>
    /// <returns>True if the module can proceed with execution, false if constraints prevent execution.</returns>
    public bool MarkModuleStarted(Type moduleType)
    {
        if (IsDisposed)
        {
            return false;
        }

        return _stateTracker.MarkModuleStarted(moduleType);
    }

    /// <summary>
    /// Marks a module as completed and notifies dependents.
    /// </summary>
    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null, ModuleStatus? statusOverride = null)
    {
        if (IsDisposed)
        {
            return;
        }

        _stateTracker.MarkModuleCompleted(moduleType, success, exception, statusOverride);
    }

    /// <summary>
    /// Gets the completion task for a specific module.
    /// </summary>
    public Task<IModule>? GetModuleCompletionTask(Type moduleType)
    {
        return _stateTracker.GetModuleCompletionTask(moduleType);
    }

    /// <summary>
    /// Gets the state for a specific module.
    /// </summary>
    public ModuleState? GetModuleState(Type moduleType)
    {
        return _stateTracker.GetModuleState(moduleType);
    }

    /// <summary>
    /// Gets statistics about the current scheduler state.
    /// </summary>
    public (int Total, int Queued, int Executing, int Completed, int Pending) GetStatistics()
    {
        _stateLock.EnterReadLock();
        try
        {
            var snapshot = _stateCounters.CreateSnapshot();
            return (snapshot.Total, snapshot.Queued, snapshot.Executing, snapshot.Completed, snapshot.Pending);
        }
        finally
        {
            _stateLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Cancels all modules that are queued or pending (not yet executing).
    /// This cancels only the scheduler's internal completion sources. Call
    /// <c>RegisterTerminatedResultsForCancelledModules</c> for the returned modules to complete their public result tasks.
    /// Note: AlwaysRun modules are not cancelled as they should be allowed to complete.
    /// </summary>
    public IReadOnlyList<IModule> CancelPendingModules()
    {
        if (IsDisposed)
        {
            return [];
        }

        return _stateTracker.CancelPendingModules();
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposeState, 1) != 0)
        {
            return;
        }

        // Wake the scheduler so it observes disposal promptly. The lock and semaphore
        // deliberately remain undisposed because in-flight workers may still be using
        // them while unwinding; they become collectible with this scheduler.
        _schedulerNotification.Release();
    }

    private void AddModuleStates(IEnumerable<IModule> modules)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            if (_moduleStates.TryAdd(moduleType, new ModuleState(module, moduleType)))
            {
                _stateCounters.AddPendingModule();
            }
        }
    }

    private void FinalizeModuleMetadata()
    {
        foreach (var state in _moduleStates.Values)
        {
            _metadataRegistry.FinalizeMetadata(state.ModuleType, state.Module);
        }
    }

    private void ConfigureScheduling(ModuleState state)
    {
        var moduleType = state.ModuleType;
        var configuration = state.Module.Configuration;
        var parallelConstraintKeys = configuration.ParallelConstraintKeys
                                     ?? moduleType.GetCustomAttribute<NotInParallelAttribute>(inherit: true)?.ConstraintKeys.ToArray();

        ApplyParallelConstraints(state, parallelConstraintKeys);

        var priority = configuration.Priority
                       ?? moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true)?.Priority;
        if (priority is { } modulePriority)
        {
            state.Priority = modulePriority;
            _logger.LogDebug(
                "Module {ModuleName} has priority: {Priority}",
                moduleType.Name,
                state.Priority);
        }

        var executionHint = configuration.ExecutionHint
                            ?? moduleType.GetCustomAttribute<ExecutionHintAttribute>(inherit: true)?.ExecutionHint;
        if (executionHint is { } moduleExecutionHint)
        {
            state.ExecutionHint = moduleExecutionHint;
            _logger.LogDebug(
                "Module {ModuleName} has execution hint: {ExecutionHint}",
                moduleType.Name,
                state.ExecutionHint);
        }
    }

    private void ApplyParallelConstraints(ModuleState state, IReadOnlyList<string>? constraintKeys)
    {
        if (constraintKeys is null)
        {
            return;
        }

        if (constraintKeys.Count == 0)
        {
            state.RequiresSequentialExecution = true;
            _hasSchedulingConstraints = true;
            _logger.LogDebug(
                "Module {ModuleName} requires sequential execution (NotInParallel)",
                state.ModuleType.Name);
            return;
        }

        state.RequiredLockKeys = [.. constraintKeys];
        _hasSchedulingConstraints = true;
        _logger.LogDebug(
            "Module {ModuleName} requires locks: {Keys}",
            state.ModuleType.Name,
            string.Join(", ", state.RequiredLockKeys));
    }

    private void ConfigureDependencies(
        ModuleState state,
        IReadOnlyList<Type> availableModuleTypes)
    {
        var dependencies = ModuleStateDependencyInitializer.Populate(
            state,
            availableModuleTypes,
            _dependencyRegistry,
            _metadataRegistry);
        foreach (var (dependencyType, optional) in dependencies)
        {
            LinkDependencyState(state, dependencyType, optional);
        }
    }

    private void LinkDependencyState(
        ModuleState state,
        Type dependencyType,
        bool optional)
    {
        if (_moduleStates.TryGetValue(dependencyType, out var dependencyState))
        {
            if (dependencyState.State != ModuleExecutionState.Completed
                && state.UnresolvedDependencies.Add(dependencyType))
            {
                dependencyState.DependentModules.Add(state);
            }

            return;
        }

        if (!optional)
        {
            _logger.LogWarning(
                "Module {ModuleName} depends on {DependencyName} which is not registered",
                state.ModuleType.Name,
                dependencyType.Name);
        }
    }

    private async Task RunSchedulerLoopAsync(CancellationToken cancellationToken)
    {
        while (ShouldContinueScheduling(cancellationToken))
        {
            var queuedCount = await FindAndQueueReadyModulesAsync(cancellationToken).ConfigureAwait(false);

            if (ShouldExitScheduler(queuedCount, out var requiresDeadlockConfirmation))
            {
                _logger.LogDebug("All modules scheduled, completing scheduler");
                break;
            }

            if (queuedCount == 0)
            {
                _statusReporter.LogStatusIfIntervalElapsed(_stateQueries, _stateLock);
            }

            await WaitForNextSchedulingOpportunity(requiresDeadlockConfirmation, cancellationToken).ConfigureAwait(false);
        }
    }

    private bool ShouldContinueScheduling(CancellationToken cancellationToken)
    {
        return !IsDisposed && !cancellationToken.IsCancellationRequested;
    }

    private bool ShouldExitScheduler(int queuedCount, out bool requiresDeadlockConfirmation)
    {
        // Use a single write lock since we may need to modify _schedulerCompleted
        // This avoids the inefficient pattern of read lock -> release -> write lock
        ModuleStateSnapshot snapshot;
        bool shouldExit;
        bool isDeadlocked;
        string[] pendingModules;

        _stateLock.EnterWriteLock();
        try
        {
            snapshot = _stateCounters.CreateSnapshot();
            shouldExit = _exitConditions.ShouldExit(snapshot, queuedCount);
            var isPotentiallyDeadlocked = _exitConditions.IsDeadlocked(snapshot, queuedCount);
            isDeadlocked = shouldExit && isPotentiallyDeadlocked;
            requiresDeadlockConfirmation = isPotentiallyDeadlocked && !shouldExit;
            pendingModules = isDeadlocked
                ? _moduleStates.Values
                    .Where(x => x.State == ModuleExecutionState.Pending)
                    .Select(x => x.ModuleType.Name)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray()
                : [];

            if (shouldExit)
            {
                _schedulerCompleted = true;
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Fail outside lock to avoid holding lock while constructing the exception.
        if (isDeadlocked)
        {
            throw new DependencyCollisionException(
                $"Scheduler deadlock detected with {snapshot.Pending} pending module(s): {string.Join(", ", pendingModules)}. " +
                "Check for circular dependencies or missing module registrations.");
        }

        return shouldExit;
    }

    private async Task WaitForNextSchedulingOpportunity(
        bool requiresDeadlockConfirmation,
        CancellationToken cancellationToken)
    {
        try
        {
            if (requiresDeadlockConfirmation)
            {
                await _schedulerNotification.WaitAsync(_options.NotificationTimeout, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _schedulerNotification.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
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
        // Use copy-on-read pattern: collect data inside lock, process outside
        // This prevents LockRecursionException if metrics collector or constraint
        // evaluator callbacks try to access scheduler state
        List<ModuleState> modulesToQueue;
        List<(Type ModuleType, DateTimeOffset ReadyTime, ModulePriority Priority, ExecutionHint ExecutionHint, DateTimeOffset QueuedTime)> metricsData;

        _stateLock.EnterWriteLock();
        try
        {
            // Sort by priority descending so higher priority modules are queued first
            var potentiallyReadyModules = _pendingReadyModules
                .OrderByDescending(m => (int) m.Priority)
                .ToArray();

            // Constraint-free pipelines need no active-module scan or pairwise evaluation.
            // When constraints exist, keep one list and append modules queued in this cycle.
            List<ModuleState>? activeModules = null;
            if (_hasSchedulingConstraints)
            {
                activeModules = new List<ModuleState>(_queuedModules.Count + _executingModules.Count);
                activeModules.AddRange(_queuedModules);
                activeModules.AddRange(_executingModules);
            }

            modulesToQueue = new List<ModuleState>();
            metricsData = new List<(Type, DateTimeOffset, ModulePriority, ExecutionHint, DateTimeOffset)>();

            foreach (var moduleState in potentiallyReadyModules)
            {
                if (activeModules is not null && !_constraintEvaluator.CanQueue(moduleState, activeModules))
                {
                    continue;
                }

                // Set ReadyTime if not already set (for modules with no dependencies)
                var now = _timeProvider.GetUtcNow();
                moduleState.ReadyTime ??= now;

                // Collect metrics data for recording outside lock
                metricsData.Add((moduleState.ModuleType, moduleState.ReadyTime.Value, moduleState.Priority, moduleState.ExecutionHint, now));

                _stateCounters.Transition(moduleState.State, ModuleExecutionState.Queued);
                moduleState.State = ModuleExecutionState.Queued;
                moduleState.QueuedTime = now;
                _pendingReadyModules.Remove(moduleState);
                _queuedModules.Add(moduleState);

                activeModules?.Add(moduleState);

                modulesToQueue.Add(moduleState);
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Record metrics outside lock to prevent lock recursion
        foreach (var (moduleType, readyTime, priority, executionHint, queuedTime) in metricsData)
        {
            _metricsCollector.RecordModuleReady(moduleType, readyTime, priority, executionHint);
            _metricsCollector.RecordModuleQueued(moduleType, queuedTime);
        }

        return modulesToQueue;
    }

    private async Task QueueModulesForExecutionAsync(List<ModuleState> modulesToQueue, CancellationToken cancellationToken)
    {
        // Queue to channel outside lock (async operation)
        foreach (var moduleState in modulesToQueue)
        {
            await _readyChannel.Writer.WriteAsync(moduleState, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug(
                "Queued module {ModuleName} for execution",
                moduleState.ModuleType.Name);
        }
    }

    private void LogQueuedModules(List<ModuleState> modulesToQueue)
    {
        _logger.LogDebug(
            "Scheduler found {Count} ready modules: {Modules}",
            modulesToQueue.Count,
            string.Join(", ", modulesToQueue.Select(m => m.ModuleType.Name)));
    }
}
