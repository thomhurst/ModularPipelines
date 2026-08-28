using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing bash commands and scripts.
/// </summary>
/// <remarks>
/// This interface is used for running bash shell commands on Unix-like systems.
/// For PowerShell execution, see <see cref="IPowerShellContext"/>.
/// For general command line tool execution, see <see cref="ICommandContext"/>.
/// </remarks>
public interface IBashContext
{
    /// <summary>
    /// Executes a bash script.
    /// </summary>
    /// <param name="script">The bash script to execute.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Shell.Bash.RunAsync("echo 'Hello World'");
    /// Console.WriteLine(result.StandardOutput);
    /// </code>
    /// </example>
    Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a bash script using the supplied options.
    /// </summary>
    /// <param name="options">The bash command options containing the script to execute.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    Task<CommandResult> RunAsync(
        BashCommandOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a bash script from a file.
    /// </summary>
    /// <param name="path">The path to the bash script file.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Shell.Bash.RunFileAsync("/path/to/script.sh");
    /// Console.WriteLine(result.StandardOutput);
    /// </code>
    /// </example>
    Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a bash script file using the supplied options.
    /// </summary>
    /// <param name="options">The bash file options containing the path to the script file.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    Task<CommandResult> RunFileAsync(
        BashFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
