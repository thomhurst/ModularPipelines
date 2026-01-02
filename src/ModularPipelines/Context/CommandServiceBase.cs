using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Base class for command-line tool service implementations.
/// </summary>
/// <remarks>
/// This base class provides a common pattern for services that wrap command-line tools.
/// It reduces code duplication by providing a protected method that handles the common
/// command execution pattern. Derived classes can use <see cref="ExecuteAsync{TOptions}"/>
/// to execute commands with strongly-typed options.
/// </remarks>
[ExcludeFromCodeCoverage]
public abstract class CommandServiceBase
{
    /// <summary>
    /// The command execution service.
    /// </summary>
    protected readonly ICommand Command;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandServiceBase"/> class.
    /// </summary>
    /// <param name="command">The command execution service.</param>
    protected CommandServiceBase(ICommand command)
    {
        Command = command;
    }

    /// <summary>
    /// Executes a command-line tool with the specified options.
    /// </summary>
    /// <typeparam name="TOptions">The type of command options.</typeparam>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation with the command result.</returns>
    protected virtual async Task<CommandResult> ExecuteAsync<TOptions>(TOptions options, CancellationToken cancellationToken = default)
        where TOptions : CommandLineToolOptions
    {
        return await Command.ExecuteCommandLineTool(options, cancellationToken).ConfigureAwait(false);
    }
}
