namespace ModularPipelines.Options;

/// <summary>
/// Specifies the verbosity level for command logging.
/// </summary>
public enum CommandLogVerbosity
{
    /// <summary>
    /// No output unless individual logging options explicitly enable it.
    /// </summary>
    Silent = 0,

    /// <summary>
    /// Command input only.
    /// </summary>
    InputOnly = 1,

    /// <summary>
    /// Standard output (default).
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Include exit code and execution time.
    /// </summary>
    Detailed = 3,

    /// <summary>
    /// Include working directory and timestamps in addition to detailed output.
    /// </summary>
    Diagnostic = 4
}
