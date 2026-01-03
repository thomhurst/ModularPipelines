using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitCommands
{
    Task<CommandResult> Add(GitAddOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Am(GitAmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Apply(GitApplyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Archive(GitArchiveOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Base(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Bisect(GitBisectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Blame(GitBlameOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Branch(GitBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Bugreport(GitBugreportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Bundle(GitBundleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CatFile(GitCatFileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CheckIgnore(GitCheckIgnoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CheckoutIndex(GitCheckoutIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Checkout(GitCheckoutOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CherryPick(GitCherryPickOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Clean(GitCleanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Clone(GitCloneOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Commit(GitCommitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CommitTree(GitCommitTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Config(GitConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> CountObjects(GitCountObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Daemon(GitDaemonOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Describe(GitDescribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> DiffIndex(GitDiffIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Diff(GitDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Difftool(GitDifftoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> FastImport(GitFastImportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Fetch(GitFetchOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> FilterBranch(GitFilterBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> ForEachRef(GitForEachRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> FormatPatch(GitFormatPatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Fsck(GitFsckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Gc(GitGcOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Grep(GitGrepOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> HashObject(GitHashObjectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Help(GitHelpOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Init(GitInitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Instaweb(GitInstawebOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Log(GitLogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> LsFiles(GitLsFilesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> LsTree(GitLsTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> MergeBase(GitMergeBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Merge(GitMergeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Mergetool(GitMergetoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Mv(GitMvOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Notes(GitNotesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Git(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Pull(GitPullOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Push(GitPushOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> RangeDiff(GitRangeDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> ReadTree(GitReadTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Rebase(GitRebaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Reflog(GitReflogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Remote(GitRemoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> RequestPull(GitRequestPullOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Reset(GitResetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Restore(GitRestoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Revert(GitRevertOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> RevList(GitRevListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> RevParse(GitRevParseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Rm(GitRmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> SendEmail(GitSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Shortlog(GitShortlogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Show(GitShowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> ShowRef(GitShowRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Stash(GitStashOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Status(GitStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Submodule(GitSubmoduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Svn(GitSvnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Switch(GitSwitchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> SymbolicRef(GitSymbolicRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Tag(GitTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> UpdateIndex(GitUpdateIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> UpdateRef(GitUpdateRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> UpdateServerInfo(GitUpdateServerInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> VerifyPack(GitVerifyPackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Worktree(GitWorktreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> WriteTree(GitWriteTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, CancellationToken cancellationToken = default);
}
