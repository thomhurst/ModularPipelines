namespace ModularPipelines;

/// <summary>
/// Provides build system-specific commands for creating collapsible log groups in CI/CD output.
/// </summary>
internal interface ISmartCollapsableLoggingStringBlockProvider
{
    /// <summary>
    /// Gets the command to start a collapsible console log group for the current build system.
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    /// <returns>
    /// The formatted start command for the current build system, or a default visual marker if not supported.
    /// </returns>
    string? GetStartConsoleLogGroup(string name);

    /// <summary>
    /// Gets the command to end a collapsible console log group for the current build system.
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    /// <returns>
    /// The formatted end command for the current build system, or null if not supported.
    /// </returns>
    string? GetEndConsoleLogGroup(string name);
}
