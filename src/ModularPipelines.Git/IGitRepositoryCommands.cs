using ModularPipelines.Git.Models;
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

public interface IGitBranchCommands
{
    Task<CommandResult> AmAsync(GitAmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> BisectAsync(GitBisectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> BranchAsync(GitBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CheckoutAsync(GitCheckoutOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CherryPickAsync(GitCherryPickOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CommitAsync(GitCommitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CommitTreeAsync(GitCommitTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> MergeAsync(GitMergeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> MergeBaseAsync(GitMergeBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> MergetoolAsync(GitMergetoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RebaseAsync(GitRebaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ResetAsync(GitResetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RevertAsync(GitRevertOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> StashAsync(GitStashOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SwitchAsync(GitSwitchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> TagAsync(GitTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}

public interface IGitRemoteCommands
{
    Task<CommandResult> FetchAsync(GitFetchOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> PullAsync(GitPullOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> PushAsync(GitPushOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RemoteAsync(GitRemoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RequestPullAsync(GitRequestPullOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SendEmailAsync(GitSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SubmoduleAsync(GitSubmoduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SvnAsync(GitSvnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}

public interface IGitHistoryCommands
{
    Task<CommandResult> BlameAsync(GitBlameOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DescribeAsync(GitDescribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ForEachRefAsync(GitForEachRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FormatPatchAsync(GitFormatPatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> LogAsync(GitLogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> NotesAsync(GitNotesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RangeDiffAsync(GitRangeDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ReflogAsync(GitReflogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RevListAsync(GitRevListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RevParseAsync(GitRevParseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShortlogAsync(GitShortlogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShowAsync(GitShowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShowRefAsync(GitShowRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SymbolicRefAsync(GitSymbolicRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> CommitsAsync(GitOptions? options = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> CommitsAsync(string? branch, GitOptions? options = null, CancellationToken cancellationToken = default);
}

public interface IGitMaintenanceCommands
{
    Task<CommandResult> CatFileAsync(GitCatFileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CountObjectsAsync(GitCountObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FastImportAsync(GitFastImportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FilterBranchAsync(GitFilterBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FsckAsync(GitFsckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> GcAsync(GitGcOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> HashObjectAsync(GitHashObjectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> LsTreeAsync(GitLsTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateRefAsync(GitUpdateRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateServerInfoAsync(GitUpdateServerInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> VerifyPackAsync(GitVerifyPackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}
