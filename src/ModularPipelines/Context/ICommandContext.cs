using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing command line tools and processes.
/// </summary>
/// <remarks>
/// This interface is the core abstraction for running external processes.
/// For shell-specific execution, see <see cref="IBashContext"/> and <see cref="IPowerShellContext"/>.
/// </remarks>
public interface ICommandContext
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The tool-specific options.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineToolAsync(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
