using System.Collections.Immutable;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.State;

/// <summary>
/// Pure functions for module state transitions.
/// </summary>
/// <remarks>
/// These functions are stateless and can be used for:
/// - Testing state transitions in isolation
/// - Validating transition legality
/// - Creating new state snapshots without side effects
/// </remarks>
internal static class ModuleStateTransitions
{
    /// <summary>
    /// Checks if a transition from one phase to another is valid.
    /// </summary>
    public static bool IsValidTransition(ModuleExecutionPhase from, ModuleExecutionPhase to)
    {
        return (from, to) switch
        {
            // From Pending
            (ModuleExecutionPhase.Pending, ModuleExecutionPhase.Queued) => true,
            (ModuleExecutionPhase.Pending, ModuleExecutionPhase.Skipped) => true,
            (ModuleExecutionPhase.Pending, ModuleExecutionPhase.Cancelled) => true,

            // From Queued
            (ModuleExecutionPhase.Queued, ModuleExecutionPhase.Running) => true,
            (ModuleExecutionPhase.Queued, ModuleExecutionPhase.Pending) => true, // Constraint failure revert
            (ModuleExecutionPhase.Queued, ModuleExecutionPhase.Skipped) => true,
            (ModuleExecutionPhase.Queued, ModuleExecutionPhase.Cancelled) => true,

            // From Running
            (ModuleExecutionPhase.Running, ModuleExecutionPhase.Completed) => true,
            (ModuleExecutionPhase.Running, ModuleExecutionPhase.Failed) => true,
            (ModuleExecutionPhase.Running, ModuleExecutionPhase.Skipped) => true,
            (ModuleExecutionPhase.Running, ModuleExecutionPhase.Cancelled) => true,

            // Terminal states cannot transition
            (ModuleExecutionPhase.Completed, _) => false,
            (ModuleExecutionPhase.Failed, _) => false,
            (ModuleExecutionPhase.Skipped, _) => false,
            (ModuleExecutionPhase.Cancelled, _) => false,

            _ => false,
        };
    }

    /// <summary>
    /// Creates a new Pending phase with a dependency removed.
    /// </summary>
    public static ModuleExecutionPhase.Pending RemoveDependency(
        ModuleExecutionPhase.Pending pending,
        Type resolvedDependency)
    {
        return pending with
        {
            UnresolvedDependencies = pending.UnresolvedDependencies.Remove(resolvedDependency),
        };
    }

    /// <summary>
    /// Creates a Queued phase from a Pending phase.
    /// </summary>
    public static ModuleExecutionPhase.Queued ToQueued(
        ModuleExecutionPhase.Pending pending,
        DateTimeOffset now)
    {
        if (!pending.IsReadyToQueue)
        {
            throw new InvalidOperationException("Cannot queue module with unresolved dependencies.");
        }

        return new ModuleExecutionPhase.Queued
        {
            DependentModules = pending.DependentModules,
            QueuedAt = now,
            ReadyAt = now,
        };
    }

    /// <summary>
    /// Creates a Running phase from a Queued phase.
    /// </summary>
    public static ModuleExecutionPhase.Running ToRunning(
        ModuleExecutionPhase.Queued queued,
        DateTimeOffset now,
        CancellationTokenSource cancellationSource)
    {
        return new ModuleExecutionPhase.Running
        {
            DependentModules = queued.DependentModules,
            StartedAt = now,
            QueuedAt = queued.QueuedAt,
            CancellationSource = cancellationSource,
        };
    }

    /// <summary>
    /// Creates a Pending phase from a Queued phase (constraint failure revert).
    /// </summary>
    public static ModuleExecutionPhase.Pending ToPending(ModuleExecutionPhase.Queued queued)
    {
        return new ModuleExecutionPhase.Pending
        {
            UnresolvedDependencies = ImmutableHashSet<Type>.Empty,
            DependentModules = queued.DependentModules,
        };
    }

    /// <summary>
    /// Creates a Completed phase from a Running phase.
    /// </summary>
    public static ModuleExecutionPhase.Completed ToCompleted(
        ModuleExecutionPhase.Running running,
        DateTimeOffset now,
        IModuleResult result)
    {
        return new ModuleExecutionPhase.Completed
        {
            DependentModules = running.DependentModules,
            StartedAt = running.StartedAt,
            CompletedAt = now,
            Result = result,
        };
    }

    /// <summary>
    /// Creates a Failed phase from a Running phase.
    /// </summary>
    public static ModuleExecutionPhase.Failed ToFailed(
        ModuleExecutionPhase.Running running,
        DateTimeOffset now,
        Exception exception,
        IModuleResult result)
    {
        return new ModuleExecutionPhase.Failed
        {
            DependentModules = running.DependentModules,
            StartedAt = running.StartedAt,
            FailedAt = now,
            Exception = exception,
            Result = result,
        };
    }

    /// <summary>
    /// Creates a Skipped phase from any non-terminal phase.
    /// </summary>
    public static ModuleExecutionPhase.Skipped ToSkipped(
        ModuleExecutionPhase phase,
        DateTimeOffset now,
        SkipDecision skipDecision,
        IModuleResult result)
    {
        var dependentModules = GetDependentModules(phase);

        return new ModuleExecutionPhase.Skipped
        {
            DependentModules = dependentModules,
            SkippedAt = now,
            SkipDecision = skipDecision,
            Result = result,
        };
    }

    /// <summary>
    /// Creates a Cancelled phase from any non-terminal phase.
    /// </summary>
    public static ModuleExecutionPhase.Cancelled ToCancelled(
        ModuleExecutionPhase phase,
        DateTimeOffset now)
    {
        var dependentModules = GetDependentModules(phase);

        return new ModuleExecutionPhase.Cancelled
        {
            DependentModules = dependentModules,
            CancelledAt = now,
            PreviousPhase = phase,
        };
    }

    /// <summary>
    /// Gets the dependent modules list from any phase.
    /// </summary>
    public static ImmutableList<Type> GetDependentModules(ModuleExecutionPhase phase)
    {
        return phase switch
        {
            ModuleExecutionPhase.Pending p => p.DependentModules,
            ModuleExecutionPhase.Queued q => q.DependentModules,
            ModuleExecutionPhase.Running r => r.DependentModules,
            ModuleExecutionPhase.Completed c => c.DependentModules,
            ModuleExecutionPhase.Failed f => f.DependentModules,
            ModuleExecutionPhase.Skipped s => s.DependentModules,
            ModuleExecutionPhase.Cancelled can => can.DependentModules,
            _ => ImmutableList<Type>.Empty,
        };
    }

    /// <summary>
    /// Gets the result from a terminal phase, if available.
    /// </summary>
    public static IModuleResult? GetResult(ModuleExecutionPhase phase)
    {
        return phase switch
        {
            ModuleExecutionPhase.Completed c => c.Result,
            ModuleExecutionPhase.Failed f => f.Result,
            ModuleExecutionPhase.Skipped s => s.Result,
            _ => null,
        };
    }

    /// <summary>
    /// Gets the exception from a Failed phase, if available.
    /// </summary>
    public static Exception? GetException(ModuleExecutionPhase phase)
    {
        return phase switch
        {
            ModuleExecutionPhase.Failed f => f.Exception,
            _ => null,
        };
    }

    /// <summary>
    /// Gets the skip decision from a Skipped phase, if available.
    /// </summary>
    public static SkipDecision? GetSkipDecision(ModuleExecutionPhase phase)
    {
        return phase switch
        {
            ModuleExecutionPhase.Skipped s => s.SkipDecision,
            _ => null,
        };
    }

    /// <summary>
    /// Checks if the phase is terminal (no further transitions possible).
    /// </summary>
    public static bool IsTerminal(ModuleExecutionPhase phase)
    {
        return phase is ModuleExecutionPhase.Completed
            or ModuleExecutionPhase.Failed
            or ModuleExecutionPhase.Skipped
            or ModuleExecutionPhase.Cancelled;
    }
}
