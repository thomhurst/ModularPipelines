// This file provides a base class for Git command services to reduce duplication.

using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <summary>
/// Base class for Git command services that provides common command execution functionality.
/// Reduces code duplication in GitCommands and related classes.
/// </summary>
public abstract class GitCommandService
{
    /// <summary>
    /// The command executor for running CLI commands.
    /// </summary>
    protected readonly ICommand Command;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitCommandService"/> class.
    /// </summary>
    /// <param name="command">The command executor.</param>
    protected GitCommandService(ICommand command)
    {
        Command = command;
    }

    /// <summary>
    /// Executes a Git command with the specified options.
    /// </summary>
    /// <typeparam name="TOptions">The type of command options.</typeparam>
    /// <param name="options">The command options.</param>
    /// <param name="executionOptions">Optional execution configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result.</returns>
    protected virtual async Task<CommandResult> ExecuteAsync<TOptions>(
        TOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
        where TOptions : CommandLineToolOptions
    {
        return await Command.ExecuteCommandLineTool(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a Git command with the specified options, creating a new instance if null.
    /// </summary>
    /// <typeparam name="TOptions">The type of command options.</typeparam>
    /// <param name="options">The command options (may be null).</param>
    /// <param name="executionOptions">Optional execution configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command result.</returns>
    protected virtual async Task<CommandResult> ExecuteWithDefaultAsync<TOptions>(
        TOptions? options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
        where TOptions : CommandLineToolOptions, new()
    {
        return await Command.ExecuteCommandLineTool(options ?? new TOptions(), executionOptions, cancellationToken).ConfigureAwait(false);
    }
}
