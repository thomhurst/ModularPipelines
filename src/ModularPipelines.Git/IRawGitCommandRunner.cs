using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <summary>
/// Provides low-level git command execution that preserves standard output exactly.
/// Implement this interface alongside <see cref="IGitCommandRunner"/> when replacing the default
/// runner so consumers can safely process binary-delimited output.
/// </summary>
public interface IRawGitCommandRunner
{
    /// <summary>
    /// Executes git commands and returns standard output without trimming or normalization.
    /// </summary>
    /// <param name="commandEnvironmentOptions">Optional command environment configuration.</param>
    /// <param name="cancellationToken">The token used to cancel command execution.</param>
    /// <param name="commands">Git command arguments to execute.</param>
    /// <returns>The exact standard output from the git command.</returns>
    Task<string> RunCommandsUntrimmed(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands);
}
