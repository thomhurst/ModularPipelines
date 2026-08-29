using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to command execution capabilities including CLI tools, Bash, and PowerShell.
/// </summary>
public interface IShellContext
{
    /// <summary>
    /// Executes a command-line tool using the supplied options.
    /// </summary>
    /// <param name="options">The command-line tool options.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result.</returns>
#pragma warning disable RS0026 // Strongly typed and raw command overloads intentionally share execution defaults.
    Task<CommandResult> RunAsync(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an arbitrary command-line tool.
    /// </summary>
    /// <param name="tool">The name or path of the command-line tool.</param>
    /// <param name="arguments">The command arguments.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        string tool,
        IReadOnlyList<string> arguments,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an arbitrary command-line tool with a cancellation token.
    /// </summary>
    /// <param name="tool">The name or path of the command-line tool.</param>
    /// <param name="arguments">The command arguments.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        string tool,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken);
#pragma warning restore RS0026

    /// <summary>
    /// Execute Bash scripts and commands.
    /// </summary>
    IBashContext Bash { get; }

    /// <summary>
    /// Execute PowerShell scripts and commands.
    /// </summary>
    IPowerShellContext PowerShell { get; }
}
