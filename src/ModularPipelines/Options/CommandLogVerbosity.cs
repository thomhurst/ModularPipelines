namespace ModularPipelines.Options;

/// <summary>
/// Specifies the verbosity level for command logging.
/// </summary>
public enum CommandLogVerbosity
{
    /// <summary>
    /// No output at all.
    /// </summary>
    Silent = 0,

    /// <summary>
    /// Only errors and warnings.
    /// </summary>
    Minimal = 1,

    /// <summary>
    /// Standard output (default).
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Include additional context like working directory and timing.
    /// </summary>
    Detailed = 3,

    /// <summary>
    /// Full verbose output for debugging.
    /// </summary>
    Diagnostic = 4
}
