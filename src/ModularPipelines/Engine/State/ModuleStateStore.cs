using System.Collections.Concurrent;
using System.Collections.Immutable;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.State;

/// <summary>
/// Thread-safe store for module state using lock-free compare-and-swap operations.
/// </summary>
/// <remarks>
/// <para>
/// This class replaces the previous ReaderWriterLockSlim-based synchronization
/// with a lock-free approach using immutable state snapshots and atomic updates.
/// </para>
/// <para>
/// <b>Thread Safety:</b> All operations are thread-safe. Reads always see a
/// consistent snapshot. Updates use compare-and-swap with automatic retry on contention.
/// </para>
/// </remarks>
internal sealed class ModuleStateStore : IModuleStateStore
{
    private readonly ConcurrentDictionary<Type, ModuleStateSnapshot> _states = new();
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Event raised when a module's state changes.
    /// </summary>
    public event Action<ModuleStateSnapshot, ModuleStateSnapshot>? StateChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleStateStore"/> class.
    /// </summary>
    public ModuleStateStore(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Registers a new module with initial Pending state.
    /// </summary>
    public ModuleStateSnapshot RegisterModule(
        IModule module,
        Type moduleType,
        ImmutableHashSet<Type> dependencies,
        ImmutableList<Type> dependentModules,
        bool requiresSequentialExecution,
        string[] requiredLockKeys,
        Enums.ModulePriority priority,
        Enums.ExecutionType executionType)
    {
        var initialState = new ModuleStateSnapshot
        {
            Module = module,
            ModuleType = moduleType,
            Phase = new ModuleExecutionPhase.Pending
            {
                UnresolvedDependencies = dependencies,
                DependentModules = dependentModules,
            },
            RequiresSequentialExecution = requiresSequentialExecution,
            RequiredLockKeys = requiredLockKeys,
            Priority = priority,
            ExecutionType = executionType,
            CompletionSource = new TaskCompletionSource<IModule>(),
        };

        if (!_states.TryAdd(moduleType, initialState))
        {
            throw new InvalidOperationException($"Module {moduleType.Name} is already registered.");
        }

        return initialState;
    }

    /// <summary>
    /// Gets the current state of a module.
    /// </summary>
    public ModuleStateSnapshot? GetState(Type moduleType)
    {
        return _states.TryGetValue(moduleType, out var state) ? state : null;
    }

    /// <summary>
    /// Gets all module states.
    /// </summary>
    public IReadOnlyCollection<ModuleStateSnapshot> GetAllStates()
    {
        return _states.Values.ToList();
    }

    /// <summary>
    /// Atomically updates a module's state using compare-and-swap.
    /// </summary>
    /// <param name="moduleType">The module type to update.</param>
    /// <param name="updateFunc">Function that takes current state and returns new state, or null to abort.</param>
    /// <returns>The new state if update succeeded, null if aborted or module not found.</returns>
    public ModuleStateSnapshot? TryUpdate(Type moduleType, Func<ModuleStateSnapshot, ModuleStateSnapshot?> updateFunc)
    {
        const int maxRetries = 100;
        var retries = 0;

        while (retries < maxRetries)
        {
            if (!_states.TryGetValue(moduleType, out var current))
            {
                return null;
            }

            var next = updateFunc(current);
            if (next == null)
            {
                return null; // Update aborted by caller
            }

            if (ReferenceEquals(current, next))
            {
                return current; // No change needed
            }

            if (_states.TryUpdate(moduleType, next, current))
            {
                StateChanged?.Invoke(current, next);
                return next;
            }

            retries++;

            // Brief yield to reduce contention
            Thread.SpinWait(1 << Math.Min(retries, 10));
        }

        throw new InvalidOperationException($"Failed to update state for {moduleType.Name} after {maxRetries} retries due to contention.");
    }

    /// <summary>
    /// Removes a resolved dependency from a module in Pending state.
    /// </summary>
    public ModuleStateSnapshot? ResolveDependency(Type moduleType, Type resolvedDependency)
    {
        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Pending pending)
            {
                return current; // Not in Pending state, no-op
            }

            if (!pending.UnresolvedDependencies.Contains(resolvedDependency))
            {
                return current; // Dependency already resolved
            }

            return current with
            {
                Phase = pending with
                {
                    UnresolvedDependencies = pending.UnresolvedDependencies.Remove(resolvedDependency),
                },
            };
        });
    }

    /// <summary>
    /// Transitions a module from Pending to Queued state.
    /// </summary>
    public ModuleStateSnapshot? TransitionToQueued(Type moduleType)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Pending pending)
            {
                return null; // Can only queue from Pending
            }

            if (!pending.IsReadyToQueue)
            {
                return null; // Dependencies not resolved
            }

            return current with
            {
                Phase = new ModuleExecutionPhase.Queued
                {
                    DependentModules = pending.DependentModules,
                    QueuedAt = now,
                    ReadyAt = now,
                },
            };
        });
    }

    /// <summary>
    /// Transitions a module from Queued to Running state.
    /// </summary>
    public ModuleStateSnapshot? TransitionToRunning(Type moduleType, CancellationTokenSource cancellationSource)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Queued queued)
            {
                return null; // Can only run from Queued
            }

            return current with
            {
                Phase = new ModuleExecutionPhase.Running
                {
                    DependentModules = queued.DependentModules,
                    StartedAt = now,
                    QueuedAt = queued.QueuedAt,
                    CancellationSource = cancellationSource,
                },
            };
        });
    }

    /// <summary>
    /// Transitions a module from Queued back to Pending (constraint failure).
    /// </summary>
    public ModuleStateSnapshot? RevertToPending(Type moduleType)
    {
        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Queued queued)
            {
                return null; // Can only revert from Queued
            }

            return current with
            {
                Phase = new ModuleExecutionPhase.Pending
                {
                    UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
                    DependentModules = queued.DependentModules,
                },
            };
        });
    }

    /// <summary>
    /// Transitions a module from Running to Completed state.
    /// </summary>
    public ModuleStateSnapshot? TransitionToCompleted(Type moduleType, Models.IModuleResult result)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Running running)
            {
                return null; // Can only complete from Running
            }

            var newState = current with
            {
                Phase = new ModuleExecutionPhase.Completed
                {
                    DependentModules = running.DependentModules,
                    StartedAt = running.StartedAt,
                    CompletedAt = now,
                    Result = result,
                },
            };

            current.CompletionSource.TrySetResult(current.Module);
            return newState;
        });
    }

    /// <summary>
    /// Transitions a module from Running to Failed state.
    /// </summary>
    public ModuleStateSnapshot? TransitionToFailed(Type moduleType, Exception exception, Models.IModuleResult result)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.Phase is not ModuleExecutionPhase.Running running)
            {
                return null; // Can only fail from Running
            }

            var newState = current with
            {
                Phase = new ModuleExecutionPhase.Failed
                {
                    DependentModules = running.DependentModules,
                    StartedAt = running.StartedAt,
                    FailedAt = now,
                    Exception = exception,
                    Result = result,
                },
            };

            current.CompletionSource.TrySetException(exception);
            return newState;
        });
    }

    /// <summary>
    /// Transitions a module to Skipped state (can happen from Pending, Queued, or Running).
    /// </summary>
    public ModuleStateSnapshot? TransitionToSkipped(Type moduleType, Models.SkipDecision skipDecision, Models.IModuleResult result)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.IsTerminal)
            {
                return null; // Already in terminal state
            }

            var dependentModules = current.Phase switch
            {
                ModuleExecutionPhase.Pending p => p.DependentModules,
                ModuleExecutionPhase.Queued q => q.DependentModules,
                ModuleExecutionPhase.Running r => r.DependentModules,
                _ => ImmutableList<Type>.Empty,
            };

            var newState = current with
            {
                Phase = new ModuleExecutionPhase.Skipped
                {
                    DependentModules = dependentModules,
                    SkippedAt = now,
                    SkipDecision = skipDecision,
                    Result = result,
                },
            };

            current.CompletionSource.TrySetResult(current.Module);
            return newState;
        });
    }

    /// <summary>
    /// Transitions a module to Cancelled state.
    /// </summary>
    public ModuleStateSnapshot? TransitionToCancelled(Type moduleType)
    {
        var now = _timeProvider.GetUtcNow();

        return TryUpdate(moduleType, current =>
        {
            if (current.IsTerminal)
            {
                return null; // Already in terminal state
            }

            var dependentModules = current.Phase switch
            {
                ModuleExecutionPhase.Pending p => p.DependentModules,
                ModuleExecutionPhase.Queued q => q.DependentModules,
                ModuleExecutionPhase.Running r => r.DependentModules,
                _ => ImmutableList<Type>.Empty,
            };

            var newState = current with
            {
                Phase = new ModuleExecutionPhase.Cancelled
                {
                    DependentModules = dependentModules,
                    CancelledAt = now,
                    PreviousPhase = current.Phase,
                },
            };

            current.CompletionSource.TrySetCanceled();
            return newState;
        });
    }

    /// <summary>
    /// Gets modules that are ready to be queued.
    /// </summary>
    public IEnumerable<ModuleStateSnapshot> GetReadyModules()
    {
        return _states.Values.Where(s => s.IsReadyToQueue);
    }

    /// <summary>
    /// Gets modules in Queued state.
    /// </summary>
    public IEnumerable<ModuleStateSnapshot> GetQueuedModules()
    {
        return _states.Values.Where(s => s.Phase is ModuleExecutionPhase.Queued);
    }

    /// <summary>
    /// Gets modules in Running state.
    /// </summary>
    public IEnumerable<ModuleStateSnapshot> GetRunningModules()
    {
        return _states.Values.Where(s => s.Phase is ModuleExecutionPhase.Running);
    }

    /// <summary>
    /// Gets modules in terminal states.
    /// </summary>
    public IEnumerable<ModuleStateSnapshot> GetCompletedModules()
    {
        return _states.Values.Where(s => s.IsTerminal);
    }

    /// <summary>
    /// Gets count of modules by state.
    /// </summary>
    public (int Pending, int Queued, int Running, int Completed) GetStateCounts()
    {
        int pending = 0, queued = 0, running = 0, completed = 0;

        foreach (var state in _states.Values)
        {
            switch (state.Phase)
            {
                case ModuleExecutionPhase.Pending:
                    pending++;
                    break;
                case ModuleExecutionPhase.Queued:
                    queued++;
                    break;
                case ModuleExecutionPhase.Running:
                    running++;
                    break;
                default:
                    if (state.IsTerminal)
                    {
                        completed++;
                    }

                    break;
            }
        }

        return (pending, queued, running, completed);
    }
}
