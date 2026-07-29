using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitRepositoryCommands
{
    Task<CommandResult> ArchiveAsync(GitArchiveOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> BaseAsync(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> BugreportAsync(GitBugreportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> BundleAsync(GitBundleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CloneAsync(GitCloneOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigAsync(GitConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DaemonAsync(GitDaemonOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> GitAsync(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> HelpAsync(GitHelpOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> InitAsync(GitInitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> InstawebAsync(GitInstawebOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}
