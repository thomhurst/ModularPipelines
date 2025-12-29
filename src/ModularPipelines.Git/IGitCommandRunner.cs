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
    Task<string> RunCommands(CommandLineOptions? commandEnvironmentOptions, params string?[] commands);

    /// <summary>
    /// Executes git commands and returns the trimmed standard output, or null if the command fails.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The trimmed standard output from the git command, or null if the command failed.</returns>
    Task<string?> RunCommandsOrNull(CommandLineOptions? commandEnvironmentOptions, params string?[] commands);
}
