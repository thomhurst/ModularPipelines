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
    /// <param name="options">The command options. Use <see cref="CommandLineToolOptions.LogSettings"/> to control logging behavior.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}
