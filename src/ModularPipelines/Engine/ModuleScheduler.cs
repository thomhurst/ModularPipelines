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
    private readonly IModuleStateTracker _stateTracker;

    private static readonly TimeSpan StatusLogInterval = TimeSpan.FromSeconds(15);

    private bool _schedulerCompleted;
    private bool _isDisposed;
    private DateTimeOffset _lastStatusLogTime;

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

        // Initialize state tracker with shared state
        _stateTracker = new ModuleStateTracker(
            logger,
            timeProvider,
            metricsCollector,
            constraintEvaluator,
            _moduleStates,
            _queuedModules,
            _executingModules,
            _stateQueries,
            _stateLock,
            _schedulerNotification,
            () => _schedulerCompleted);
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

        var moduleArray = modules.ToArray();

        foreach (var module in moduleArray)
        {
            var moduleType = module.GetType();
            var state = new ModuleState(module, moduleType);
            _moduleStates.TryAdd(moduleType, state);
        }

        // Get all available module types for DependsOnAllModulesInheritingFrom resolution
        var availableModuleTypes = _moduleStates.Keys.ToArray();

        foreach (var state in _moduleStates.Values)
        {
            var moduleType = state.ModuleType;

            // Use cached metadata to avoid repeated reflection lookups
            var metadata = ModuleMetadataCache.GetMetadata(moduleType);

            if (metadata.NotInParallelAttribute != null)
            {
                if (metadata.NotInParallelAttribute.ConstraintKeys.Length == 0)
                {
                    state.RequiresSequentialExecution = true;
                    _logger.LogDebug(
                        "Module {ModuleName} requires sequential execution (NotInParallel)",
                        MarkupFormatter.FormatModuleName(moduleType.Name));
                }
                else
                {
                    state.RequiredLockKeys = metadata.NotInParallelAttribute.ConstraintKeys;
                    _logger.LogDebug(
                        "Module {ModuleName} requires locks: {Keys}",
                        MarkupFormatter.FormatModuleName(moduleType.Name),
                        string.Join(", ", state.RequiredLockKeys));
                }
            }

            // Read priority attribute
            if (metadata.PriorityAttribute != null)
            {
                state.Priority = metadata.PriorityAttribute.Priority;
                _logger.LogDebug(
                    "Module {ModuleName} has priority: {Priority}",
                    MarkupFormatter.FormatModuleName(moduleType.Name),
                    state.Priority);
            }

            // Read execution hint attribute
            if (metadata.ExecutionHintAttribute != null)
            {
                state.ExecutionType = metadata.ExecutionHintAttribute.ExecutionType;
                _logger.LogDebug(
                    "Module {ModuleName} has execution type: {ExecutionType}",
                    MarkupFormatter.FormatModuleName(moduleType.Name),
                    state.ExecutionType);
            }

            // Use the overload that includes dynamic dependencies from registration events
            // and programmatic dependencies from DeclareDependencies method
            var dependencies = ModuleDependencyResolver.GetAllDependencies(state.Module, availableModuleTypes, _dependencyRegistry);

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
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType)
                .Concat(ModuleDependencyResolver.GetProgrammaticDependencies(module));

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
        // Use a single write lock since we may need to modify _schedulerCompleted
        // This avoids the inefficient pattern of read lock -> release -> write lock
        ModuleStateSnapshot snapshot;
        bool shouldExit;
        bool isDeadlocked;

        _stateLock.EnterWriteLock();
        try
        {
            snapshot = ModuleStateSnapshot.Create(_moduleStates.Values);
            shouldExit = _exitConditions.ShouldExit(snapshot, queuedCount);
            isDeadlocked = shouldExit && _exitConditions.IsDeadlocked(snapshot, queuedCount);

            if (shouldExit)
            {
                _schedulerCompleted = true;
            }
        }
        finally
        {
            _stateLock.ExitWriteLock();
        }

        // Log outside lock to avoid holding lock during I/O
        if (shouldExit && isDeadlocked)
        {
            _logger.LogWarning(
                "Scheduler detected deadlock: {Pending} modules pending but cannot make progress. " +
                "Check for circular dependencies or missing module registrations.",
                snapshot.Pending);
        }

        return shouldExit;
    }

    private void LogSchedulerWaitingState()
    {
        var now = _timeProvider.GetUtcNow();
        if (now - _lastStatusLogTime < StatusLogInterval)
        {
            return;
        }

        _lastStatusLogTime = now;

        // Consolidate all state queries under a single read lock to reduce contention
        // Previously this called LogPendingModules() and LogExecutingModules() separately,
        // each acquiring its own read lock
        ModuleStateStatistics stats;
        ModuleState[] pending;
        ModuleState[] executing;

        _stateLock.EnterReadLock();
        try
        {
            stats = _stateQueries.GetStatistics();
            pending = _stateQueries.GetPendingModules().ToArray();
            executing = _stateQueries.GetExecutingModules().ToArray();
        }
        finally
        {
            _stateLock.ExitReadLock();
        }

        // All logging outside lock to avoid holding lock during I/O
        _logger.LogDebug(
            "Scheduler waiting: Total={Total}, Queued={Queued}, Executing={Executing}, Completed={Completed}, Pending={Pending}",
            stats.Total, stats.Queued, stats.Executing, stats.Completed, stats.Pending);

        if (pending.Length > 0)
        {
            _logger.LogDebug("Pending modules: {Modules}",
                string.Join(", ", pending.Select(FormatModuleWithDependencyCount)));
        }

        if (executing.Length > 0)
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
        return _stateTracker.MarkModuleStarted(moduleType);
    }

    /// <summary>
    /// Marks a module as completed and notifies dependents.
    /// </summary>
    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null)
    {
        _stateTracker.MarkModuleCompleted(moduleType, success, exception);
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
        _stateTracker.CancelPendingModules();
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
                .ToArray();

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
}
