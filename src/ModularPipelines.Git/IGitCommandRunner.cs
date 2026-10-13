using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <summary>
/// Provides low-level git command execution capabilities.
/// </summary>
public interface IGitCommandRunner
{
    /// <summary>
    /// Executes git commands and returns the trimmed standard output.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command.</returns>
    /// <exception cref="Exception">Thrown when the git command fails.</exception>
    Task<string> RunCommands(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands);

    /// <summary>
    /// Executes git commands and returns the trimmed standard output.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command.</returns>
    Task<string> RunCommands(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RunCommands(commandEnvironmentOptions, commands);
    }

    /// <summary>
    /// Executes git commands and returns the trimmed standard output, or null if the command fails.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command, or null if the command failed.</returns>
    Task<string?> RunCommandsOrNull(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands);

    /// <summary>
    /// Executes git commands and returns the trimmed standard output, or null if the command fails.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command, or null if the command failed.</returns>
    Task<string?> RunCommandsOrNull(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RunCommandsOrNull(commandEnvironmentOptions, commands);
    }

    /// <summary>
    /// Executes a Git command with shared root settings, returning null on command failure.
    /// </summary>
    /// <param name="options">The root settings, command parts, and command arguments.</param>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <returns>The command output, or null if the command failed.</returns>
    /// <remarks>
    /// The default implementation renders the shared settings before command parts and delegates
    /// to the configured low-level runner. Runners may override this method for typed execution.
    /// </remarks>
    async Task<string?> RunCommandsOrNull(
        GitOptions options,
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            return await RunCommandsOrNull(
                GitCommandArguments.WithSecrets(options, commandEnvironmentOptions),
                cancellationToken,
                GitCommandArguments.Build(options)).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            return null;
        }
    }
}
