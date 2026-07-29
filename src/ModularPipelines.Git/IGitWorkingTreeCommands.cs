using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitWorkingTreeCommands
{
    Task<CommandResult> AddAsync(GitAddOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ApplyAsync(GitApplyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CheckIgnoreAsync(GitCheckIgnoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CheckoutIndexAsync(GitCheckoutIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CleanAsync(GitCleanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DiffAsync(GitDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DiffIndexAsync(GitDiffIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DifftoolAsync(GitDifftoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> GrepAsync(GitGrepOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> LsFilesAsync(GitLsFilesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> MvAsync(GitMvOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ReadTreeAsync(GitReadTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RestoreAsync(GitRestoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RmAsync(GitRmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> StatusAsync(GitStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateIndexAsync(GitUpdateIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> WorktreeAsync(GitWorktreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> WriteTreeAsync(GitWriteTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}
