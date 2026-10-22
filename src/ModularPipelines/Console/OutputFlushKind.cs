namespace ModularPipelines.Console;

/// <summary>
/// Specifies how buffered module output and completion status are rendered.
/// </summary>
internal enum OutputFlushKind
{
    /// <summary>
    /// Renders output accumulated by a module that is still running.
    /// </summary>
    Incremental,

    /// <summary>
    /// Renders the final output and completion status for a module.
    /// </summary>
    Complete,

    /// <summary>
    /// Drains all remaining output at teardown without reporting module completion.
    /// </summary>
    Drain,
}
