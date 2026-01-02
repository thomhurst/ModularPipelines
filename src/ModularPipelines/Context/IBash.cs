using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing bash commands and scripts.
/// </summary>
/// <remarks>
/// This interface is used for running bash shell commands on Unix-like systems.
/// For PowerShell execution, see <see cref="IPowershell"/>.
/// For general command line tool execution, see <see cref="ICommand"/>.
/// </remarks>
public interface IBash
{
    /// <summary>
    /// Executes a bash command string.
    /// </summary>
    /// <param name="options">The bash command options containing the command to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Bash.Command(new BashCommandOptions("echo 'Hello World'"));
    /// Console.WriteLine(result.StandardOutput);
    /// </code>
    /// </example>
    Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a bash script from a file.
    /// </summary>
    /// <param name="options">The bash file options containing the path to the script file.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, exit code, and execution details.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Bash.FromFile(new BashFileOptions("/path/to/script.sh"));
    /// Console.WriteLine(result.StandardOutput);
    /// </code>
    /// </example>
    Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default);
}
