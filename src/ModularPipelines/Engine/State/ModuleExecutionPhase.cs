using System.Collections.Immutable;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.State;

/// <summary>
/// Immutable discriminated union representing all possible states of module execution.
/// </summary>
/// <remarks>
/// <para>
/// This is an unused legacy model retained only for source and binary compatibility.
/// Runtime scheduling uses the state types in the parent <c>ModularPipelines.Engine</c>
/// namespace.
/// </para>
/// </remarks>
[Obsolete("This legacy state model is unused and will be removed in the next major version.")]
public abstract record ModuleExecutionPhase
{
    /// <summary>
    /// Module is waiting for dependencies to be resolved.
    /// </summary>
    public sealed record Pending : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets the set of dependency types that haven't completed yet.
        /// </summary>
        public required ImmutableHashSet<Type> UnresolvedDependencies { get; init; }

        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets a value indicating whether all dependencies are resolved.
        /// </summary>
        public bool IsReadyToQueue => UnresolvedDependencies.IsEmpty;
    }

    /// <summary>
    /// Module has been queued and is waiting for execution slot.
    /// </summary>
    public sealed record Queued : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the module was queued.
        /// </summary>
        public required DateTimeOffset QueuedAt { get; init; }

        /// <summary>
        /// Gets when all dependencies were satisfied and the module became ready.
        /// </summary>
        public required DateTimeOffset ReadyAt { get; init; }
    }

    /// <summary>
    /// Module is currently executing.
    /// </summary>
    public sealed record Running : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the module started executing.
        /// </summary>
        public required DateTimeOffset StartedAt { get; init; }

        /// <summary>
        /// Gets when the module was queued.
        /// </summary>
        public required DateTimeOffset QueuedAt { get; init; }

        /// <summary>
        /// Gets the cancellation token source for this module execution.
        /// </summary>
        public required CancellationTokenSource CancellationSource { get; init; }
    }

    /// <summary>
    /// Module completed successfully.
    /// </summary>
    public sealed record Completed : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the module started executing.
        /// </summary>
        public required DateTimeOffset StartedAt { get; init; }

        /// <summary>
        /// Gets when the module completed.
        /// </summary>
        public required DateTimeOffset CompletedAt { get; init; }

        /// <summary>
        /// Gets the execution duration.
        /// </summary>
        public TimeSpan Duration => CompletedAt - StartedAt;

        /// <summary>
        /// Gets the module result.
        /// </summary>
        public required IModuleResult Result { get; init; }
    }

    /// <summary>
    /// Module execution failed with an exception.
    /// </summary>
    public sealed record Failed : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the module started executing.
        /// </summary>
        public required DateTimeOffset StartedAt { get; init; }

        /// <summary>
        /// Gets when the module failed.
        /// </summary>
        public required DateTimeOffset FailedAt { get; init; }

        /// <summary>
        /// Gets the execution duration.
        /// </summary>
        public TimeSpan Duration => FailedAt - StartedAt;

        /// <summary>
        /// Gets the exception that caused the failure.
        /// </summary>
        public required Exception Exception { get; init; }

        /// <summary>
        /// Gets the module result (may contain partial result).
        /// </summary>
        public required IModuleResult Result { get; init; }
    }

    /// <summary>
    /// Module was skipped.
    /// </summary>
    public sealed record Skipped : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the skip decision was made.
        /// </summary>
        public required DateTimeOffset SkippedAt { get; init; }

        /// <summary>
        /// Gets the reason for skipping.
        /// </summary>
        public required SkipDecision SkipDecision { get; init; }

        /// <summary>
        /// Gets the module result (skipped result).
        /// </summary>
        public required IModuleResult Result { get; init; }
    }

    /// <summary>
    /// Module was cancelled before completion.
    /// </summary>
    public sealed record Cancelled : ModuleExecutionPhase
    {
        /// <summary>
        /// Gets modules that depend on this module (reverse dependencies).
        /// </summary>
        public required ImmutableList<Type> DependentModules { get; init; }

        /// <summary>
        /// Gets when the module was cancelled.
        /// </summary>
        public required DateTimeOffset CancelledAt { get; init; }

        /// <summary>
        /// Gets the previous phase before cancellation.
        /// </summary>
        public required ModuleExecutionPhase PreviousPhase { get; init; }
    }
}
