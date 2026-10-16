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
    Task<string> RunCommandsAsync(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands);

    /// <summary>
    /// Executes git commands and returns the trimmed standard output.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command.</returns>
    Task<string> RunCommandsAsync(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RunCommandsAsync(commandEnvironmentOptions, commands);
    }

    /// <summary>
    /// Executes git commands and returns the trimmed standard output, or null if the command fails.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command, or null if the command failed.</returns>
    Task<string?> RunCommandsOrNullAsync(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands);

    /// <summary>
    /// Executes git commands and returns the trimmed standard output, or null if the command fails.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command, or null if the command failed.</returns>
    Task<string?> RunCommandsOrNullAsync(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RunCommandsOrNullAsync(commandEnvironmentOptions, commands);
    }

    /// <summary>
    /// Executes a Git command with shared root settings and returns trimmed standard output.
    /// </summary>
    /// <param name="options">The root settings, command parts, and command arguments.</param>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <returns>The trimmed standard output.</returns>
    Task<string> RunCommandsAsync(
        GitOptions options,
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RunCommandsAsync(
            GitCommandArguments.WithSecrets(options, commandEnvironmentOptions),
            cancellationToken,
            GitCommandArguments.Build(options));
    }

    /// <summary>
    /// Executes a Git command with shared root settings and returns trimmed standard output, or null on command failure.
    /// </summary>
    /// <param name="options">The root settings, command parts, and command arguments.</param>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <returns>The trimmed standard output, or null if the command failed.</returns>
    /// <remarks>
    /// The default implementation renders the shared settings before command parts and delegates
    /// to the configured low-level runner. Runners may override this method for typed execution.
    /// </remarks>
    async Task<string?> RunCommandsOrNullAsync(
        GitOptions options,
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            return await RunCommandsOrNullAsync(
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
