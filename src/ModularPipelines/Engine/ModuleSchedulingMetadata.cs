using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

/// <summary>
/// Cached metadata for module scheduling attributes.
/// </summary>
internal sealed record ModuleSchedulingMetadata
{
    /// <summary>
    /// Gets the NotInParallel attribute if present on the module type.
    /// </summary>
    public NotInParallelAttribute? NotInParallelAttribute { get; init; }

    /// <summary>
    /// Gets the Priority attribute if present on the module type.
    /// </summary>
    public PriorityAttribute? PriorityAttribute { get; init; }

    /// <summary>
    /// Gets the ExecutionHint attribute if present on the module type.
    /// </summary>
    public ExecutionHintAttribute? ExecutionHintAttribute { get; init; }
}
