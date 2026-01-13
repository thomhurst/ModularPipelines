using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.State;

/// <summary>
/// Immutable snapshot of a module's complete state at a point in time.
/// </summary>
/// <remarks>
/// <para>
/// This record combines identity, execution phase, and constraint information
/// into a single immutable object. State transitions create new snapshots,
/// enabling lock-free concurrent access.
/// </para>
/// <para>
/// <b>Thread Safety:</b> Fully immutable. Safe to read from any thread without synchronization.
/// </para>
/// </remarks>
public sealed record ModuleStateSnapshot
{
    /// <summary>
    /// Gets the module instance.
    /// </summary>
    public required IModule Module { get; init; }

    /// <summary>
    /// Gets the concrete type of the module.
    /// </summary>
    public required Type ModuleType { get; init; }

    /// <summary>
    /// Gets the current execution phase.
    /// </summary>
    public required ModuleExecutionPhase Phase { get; init; }

    /// <summary>
    /// Gets a value indicating whether this module requires sequential execution (no other modules executing).
    /// </summary>
    public required bool RequiresSequentialExecution { get; init; }

    /// <summary>
    /// Gets the lock keys that this module requires (for keyed NotInParallel constraints).
    /// </summary>
    public required string[] RequiredLockKeys { get; init; }

    /// <summary>
    /// Gets the execution priority.
    /// </summary>
    public required ModulePriority Priority { get; init; }

    /// <summary>
    /// Gets the execution type hint for resource-based throttling.
    /// </summary>
    public required ExecutionType ExecutionType { get; init; }

    /// <summary>
    /// Gets the completion source to signal when execution finishes.
    /// </summary>
    /// <remarks>
    /// Note: TaskCompletionSource is mutable by design, but we only expose it
    /// for signaling completion. The actual state is tracked by Phase.
    /// </remarks>
    public required TaskCompletionSource<IModule> CompletionSource { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module is ready to be queued (all dependencies resolved).
    /// </summary>
    public bool IsReadyToQueue => Phase is ModuleExecutionPhase.Pending { IsReadyToQueue: true };

    /// <summary>
    /// Gets a value indicating whether the module is in a terminal state (completed, failed, skipped, cancelled).
    /// </summary>
    public bool IsTerminal => Phase is ModuleExecutionPhase.Completed
        or ModuleExecutionPhase.Failed
        or ModuleExecutionPhase.Skipped
        or ModuleExecutionPhase.Cancelled;

    /// <summary>
    /// Gets a value indicating whether the module completed successfully.
    /// </summary>
    public bool IsSuccessful => Phase is ModuleExecutionPhase.Completed;

    /// <summary>
    /// Gets the execution status for compatibility with existing code.
    /// </summary>
    public Status Status => Phase switch
    {
        ModuleExecutionPhase.Pending => Status.NotYetStarted,
        ModuleExecutionPhase.Queued => Status.NotYetStarted,
        ModuleExecutionPhase.Running => Status.Processing,
        ModuleExecutionPhase.Completed => Status.Successful,
        ModuleExecutionPhase.Failed => Status.Failed,
        ModuleExecutionPhase.Skipped => Status.Skipped,
        ModuleExecutionPhase.Cancelled => Status.PipelineTerminated,
        _ => Status.Unknown,
    };
}
