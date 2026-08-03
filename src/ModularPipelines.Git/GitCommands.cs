using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

[ExcludeFromCodeCoverage]
public class GitCommands :
    IGitCommands,
    IGitRepositoryCommands,
    IGitWorkingTreeCommands,
    IGitBranchCommands,
    IGitRemoteCommands,
    IGitHistoryCommands,
    IGitMaintenanceCommands
{
    private readonly ICommandContext _command;
    private readonly IGitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitCommands(ICommandContext command, IGitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    public IGitRepositoryCommands Repository => this;

    public IGitWorkingTreeCommands WorkingTree => this;

    public IGitBranchCommands Branches => this;

    public IGitRemoteCommands Remotes => this;

    public IGitHistoryCommands History => this;

    public IGitMaintenanceCommands Maintenance => this;

    /// <inheritdoc/>
    public virtual async Task<CommandResult> AddAsync(GitAddOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> AmAsync(GitAmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ApplyAsync(GitApplyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ArchiveAsync(GitArchiveOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BaseAsync(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BisectAsync(GitBisectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BlameAsync(GitBlameOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BranchAsync(GitBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BugreportAsync(GitBugreportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BundleAsync(GitBundleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CatFileAsync(GitCatFileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckIgnoreAsync(GitCheckIgnoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckoutIndexAsync(GitCheckoutIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckoutAsync(GitCheckoutOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CherryPickAsync(GitCherryPickOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CleanAsync(GitCleanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CloneAsync(GitCloneOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CommitAsync(GitCommitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CommitTreeAsync(GitCommitTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ConfigAsync(GitConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CountObjectsAsync(GitCountObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DaemonAsync(GitDaemonOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DescribeAsync(GitDescribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DiffIndexAsync(GitDiffIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DiffAsync(GitDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DifftoolAsync(GitDifftoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FastImportAsync(GitFastImportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FetchAsync(GitFetchOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new GitFetchOptions(), executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FilterBranchAsync(GitFilterBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ForEachRefAsync(GitForEachRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FormatPatchAsync(GitFormatPatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FsckAsync(GitFsckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> GcAsync(GitGcOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new GitGcOptions(), executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> GrepAsync(GitGrepOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> HashObjectAsync(GitHashObjectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> HelpAsync(GitHelpOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> InitAsync(GitInitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> InstawebAsync(GitInstawebOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LogAsync(GitLogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsFilesAsync(GitLsFilesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsTreeAsync(GitLsTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MergeBaseAsync(GitMergeBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MergeAsync(GitMergeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MergetoolAsync(GitMergetoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MvAsync(GitMvOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> NotesAsync(GitNotesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> GitAsync(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> PullAsync(GitPullOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new GitPullOptions(), executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> PushAsync(GitPushOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new GitPushOptions(), executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RangeDiffAsync(GitRangeDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ReadTreeAsync(GitReadTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RebaseAsync(GitRebaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ReflogAsync(GitReflogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RemoteAsync(GitRemoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RequestPullAsync(GitRequestPullOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ResetAsync(GitResetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RestoreAsync(GitRestoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevertAsync(GitRevertOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevListAsync(GitRevListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevParseAsync(GitRevParseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RmAsync(GitRmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SendEmailAsync(GitSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ShortlogAsync(GitShortlogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ShowAsync(GitShowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ShowRefAsync(GitShowRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StashAsync(GitStashOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StatusAsync(GitStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SubmoduleAsync(GitSubmoduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SvnAsync(GitSvnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SwitchAsync(GitSwitchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SymbolicRefAsync(GitSymbolicRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> TagAsync(GitTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateIndexAsync(GitUpdateIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateRefAsync(GitUpdateRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateServerInfoAsync(GitUpdateServerInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> VerifyPackAsync(GitVerifyPackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorktreeAsync(GitWorktreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WriteTreeAsync(GitWriteTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<GitCommit> CommitsAsync(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CommitsAsync(null, options, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<GitCommit> CommitsAsync(string? branch, GitOptions? options = null, CancellationToken cancellationToken = default) =>
        GitCommitPager.EnumerateAsync(
            _gitCommandRunner,
            _gitCommitMapper,
            branch,
            null,
            cancellationToken);
}
