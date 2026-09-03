namespace ModularPipelines.Distributed;

/// <summary>
/// Reports a distributed worker's liveness and execution telemetry.
/// </summary>
public record WorkerStatus(
    int WorkerIndex)
{
    /// <summary>
    /// Gets the worker's final count of commands executed outside a module context, when available.
    /// </summary>
    public int? UnattributedCommandCount { get; init; }

    /// <summary>
    /// Gets the worker's final command counts by stable module type identifier, when available.
    /// </summary>
    public IReadOnlyDictionary<string, int>? ModuleCommandCounts { get; init; }
}
