using ModularPipelines.Cmd.Models;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd;

/// <summary>
/// Provides functionality for executing Windows command scripts.
/// </summary>
public interface ICmd
{
    /// <summary>
    /// Executes a Windows command script.
    /// </summary>
    /// <param name="script">The command script to execute.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a Windows command script using the supplied options.
    /// </summary>
    /// <param name="options">The command script options.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        CmdScriptOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a Windows command script from a file.
    /// </summary>
    /// <param name="path">The path to the command script file.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
