using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd;

/// <summary>
/// Executes inline Windows Command Prompt scripts and batch files.
/// </summary>
public interface ICmdContext
{
#pragma warning disable RS0026 // String and strongly typed overloads intentionally share execution defaults.
    /// <summary>
    /// Executes an inline Command Prompt script.
    /// </summary>
    /// <param name="script">The script to execute.</param>
    /// <param name="executionOptions">The execution options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an inline Command Prompt script.
    /// </summary>
    /// <param name="options">The script options.</param>
    /// <param name="executionOptions">The execution options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunAsync(
        CmdScriptOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a batch file.
    /// </summary>
    /// <param name="path">The path to the batch file.</param>
    /// <param name="executionOptions">The execution options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a batch file.
    /// </summary>
    /// <param name="options">The batch-file options.</param>
    /// <param name="executionOptions">The execution options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The command result.</returns>
    Task<CommandResult> RunFileAsync(
        CmdFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
#pragma warning restore RS0026
}
