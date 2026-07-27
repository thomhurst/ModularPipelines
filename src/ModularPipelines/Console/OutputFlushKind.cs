namespace ModularPipelines.Console;

/// <summary>
/// Specifies whether buffered module output is being rendered during execution or at completion.
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
}
