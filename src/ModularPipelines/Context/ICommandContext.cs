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
    /// <param name="cancellationToken">
    /// A token to cancel the command. If it cannot be canceled (including <see cref="CancellationToken.None"/>),
    /// the current module's cancellation token is used, including its current attempt timeout.
    /// Outside a module execution context, no module token is applied. An explicit cancelable token takes precedence.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineToolAsync(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
