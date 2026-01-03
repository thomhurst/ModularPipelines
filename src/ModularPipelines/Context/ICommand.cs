using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing command line tools and processes.
/// </summary>
/// <remarks>
/// This interface is the core abstraction for running external processes.
/// For shell-specific execution, see <see cref="IBash"/> and <see cref="IPowershell"/>.
/// </remarks>
public interface ICommand
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The tool-specific options.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
