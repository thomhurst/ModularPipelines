using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Executes a <see cref="CommandLine"/> and returns the result.
/// This handles all side effects: process execution, I/O capture, timeouts.
/// </summary>
/// <remarks>
/// This interface works with <see cref="ICommandLineBuilder"/> to separate command construction
/// from execution. The builder creates a <see cref="CommandLine"/>, and this executor runs it.
/// </remarks>
public interface ICommandLineExecutor
{
    /// <summary>
    /// Executes the command line.
    /// </summary>
    /// <param name="commandLine">The command to execute.</param>
    /// <param name="options">Execution options (working directory, environment, timeout).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result including exit code and output.</returns>
    Task<CommandResult> ExecuteAsync(
        CommandLine commandLine,
        CommandExecutionOptions? options = null,
        CancellationToken cancellationToken = default);
}
