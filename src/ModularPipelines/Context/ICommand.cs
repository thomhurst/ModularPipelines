using System.ComponentModel;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface ICommand
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The command options. Use <see cref="CommandLineToolOptions.LogSettings"/> to control logging behavior.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute a command line tool with custom logging options.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="loggingOptions">The logging options for this command execution. Prefer using <see cref="CommandLineToolOptions.LogSettings"/> instead.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Obsolete("Set LogSettings on CommandLineToolOptions instead. This overload will be removed in a future version.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CommandLoggingOptions? loggingOptions, CancellationToken cancellationToken = default);
}