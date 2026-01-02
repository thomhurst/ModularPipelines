using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to shell and command execution helpers.
/// </summary>
/// <remarks>
/// This context groups related shell services (Powershell, Bash, Command)
/// to reduce constructor parameter count in PipelineContext while maintaining the same public API.
/// </remarks>
public interface IShellContext
{
    /// <summary>
    /// Gets helpers for running powershell.
    /// </summary>
    IPowershell Powershell { get; }

    /// <summary>
    /// Gets helpers for executing bash commands.
    /// </summary>
    IBash Bash { get; }

    /// <summary>
    /// Gets helpers for running commands.
    /// </summary>
    /// <remarks>
    /// Provides cross-platform command execution with output capture and logging.
    /// </remarks>
    ICommand Command { get; }
}
