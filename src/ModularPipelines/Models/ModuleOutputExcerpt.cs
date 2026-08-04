namespace ModularPipelines.Models;

/// <summary>
/// Contains a size-capped tail of a module's masked output.
/// </summary>
public sealed record ModuleOutputExcerpt
{
    /// <summary>
    /// Gets the retained standard-output and informational-log tail.
    /// </summary>
    public string? StdoutTail { get; init; }

    /// <summary>
    /// Gets the retained standard-error tail.
    /// </summary>
    public string? StderrTail { get; init; }

    /// <summary>
    /// Gets the number of UTF-8 bytes discarded from the start of the combined output.
    /// </summary>
    public long TruncatedBytes { get; init; }
}
