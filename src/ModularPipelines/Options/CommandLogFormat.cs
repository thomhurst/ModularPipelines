namespace ModularPipelines.Options;

/// <summary>
/// Specifies the output format for command logging.
/// </summary>
public enum CommandLogFormat
{
    /// <summary>
    /// Plain text output (default).
    /// </summary>
    Text = 0,

    /// <summary>
    /// Structured JSON output.
    /// </summary>
    Structured = 1
}
