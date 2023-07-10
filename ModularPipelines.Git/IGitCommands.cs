using ModularPipelines.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGitCommands
{
    Task<CommandResult> Add(GitAddOptions options, CancellationToken token = default);

    Task<CommandResult> Am(GitAmOptions options, CancellationToken token = default);

    Task<CommandResult> Apply(GitApplyOptions options, CancellationToken token = default);

    Task<CommandResult> Archive(GitArchiveOptions options, CancellationToken token = default);

    Task<CommandResult> Base(GitBaseOptions options, CancellationToken token = default);

    Task<CommandResult> Bisect(GitBisectOptions options, CancellationToken token = default);

    Task<CommandResult> Blame(GitBlameOptions options, CancellationToken token = default);

    Task<CommandResult> Branch(GitBranchOptions options, CancellationToken token = default);

    Task<CommandResult> Bugreport(GitBugreportOptions options, CancellationToken token = default);

    Task<CommandResult> Bundle(GitBundleOptions options, CancellationToken token = default);

    Task<CommandResult> CatFile(GitCatFileOptions options, CancellationToken token = default);

    Task<CommandResult> CheckIgnore(GitCheckIgnoreOptions options, CancellationToken token = default);

    Task<CommandResult> CheckoutIndex(GitCheckoutIndexOptions options, CancellationToken token = default);

    Task<CommandResult> Checkout(GitCheckoutOptions options, CancellationToken token = default);

    Task<CommandResult> CherryPick(GitCherryPickOptions options, CancellationToken token = default);

    Task<CommandResult> Clean(GitCleanOptions options, CancellationToken token = default);

    Task<CommandResult> Clone(GitCloneOptions options, CancellationToken token = default);

    Task<CommandResult> Commit(GitCommitOptions options, CancellationToken token = default);

    Task<CommandResult> CommitTree(GitCommitTreeOptions options, CancellationToken token = default);

    Task<CommandResult> Config(GitConfigOptions options, CancellationToken token = default);

    Task<CommandResult> CountObjects(GitCountObjectsOptions options, CancellationToken token = default);

    Task<CommandResult> Daemon(GitDaemonOptions options, CancellationToken token = default);

    Task<CommandResult> Describe(GitDescribeOptions options, CancellationToken token = default);

    Task<CommandResult> DiffIndex(GitDiffIndexOptions options, CancellationToken token = default);

    Task<CommandResult> Diff(GitDiffOptions options, CancellationToken token = default);

    Task<CommandResult> Difftool(GitDifftoolOptions options, CancellationToken token = default);

    Task<CommandResult> FastImport(GitFastImportOptions options, CancellationToken token = default);

    Task<CommandResult> Fetch(GitFetchOptions? options = default, CancellationToken token = default);

    Task<CommandResult> FilterBranch(GitFilterBranchOptions options, CancellationToken token = default);

    Task<CommandResult> ForEachRef(GitForEachRefOptions options, CancellationToken token = default);

    Task<CommandResult> FormatPatch(GitFormatPatchOptions options, CancellationToken token = default);

    Task<CommandResult> Fsck(GitFsckOptions options, CancellationToken token = default);

    Task<CommandResult> Gc(GitGcOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Grep(GitGrepOptions options, CancellationToken token = default);

    Task<CommandResult> HashObject(GitHashObjectOptions options, CancellationToken token = default);

    Task<CommandResult> Help(GitHelpOptions options, CancellationToken token = default);

    Task<CommandResult> Init(GitInitOptions options, CancellationToken token = default);

    Task<CommandResult> Instaweb(GitInstawebOptions options, CancellationToken token = default);

    Task<CommandResult> Log(GitLogOptions options, CancellationToken token = default);

    Task<CommandResult> LsFiles(GitLsFilesOptions options, CancellationToken token = default);

    Task<CommandResult> LsTree(GitLsTreeOptions options, CancellationToken token = default);

    Task<CommandResult> MergeBase(GitMergeBaseOptions options, CancellationToken token = default);

    Task<CommandResult> Merge(GitMergeOptions options, CancellationToken token = default);

    Task<CommandResult> Mergetool(GitMergetoolOptions options, CancellationToken token = default);

    Task<CommandResult> Mv(GitMvOptions options, CancellationToken token = default);

    Task<CommandResult> Notes(GitNotesOptions options, CancellationToken token = default);

    Task<CommandResult> Git(GitBaseOptions options, CancellationToken token = default);

    Task<CommandResult> Pull(GitPullOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Push(GitPushOptions? options = default, CancellationToken token = default);

    Task<CommandResult> RangeDiff(GitRangeDiffOptions options, CancellationToken token = default);

    Task<CommandResult> ReadTree(GitReadTreeOptions options, CancellationToken token = default);

    Task<CommandResult> Rebase(GitRebaseOptions options, CancellationToken token = default);

    Task<CommandResult> Reflog(GitReflogOptions options, CancellationToken token = default);

    Task<CommandResult> Remote(GitRemoteOptions options, CancellationToken token = default);

    Task<CommandResult> RequestPull(GitRequestPullOptions options, CancellationToken token = default);

    Task<CommandResult> Reset(GitResetOptions options, CancellationToken token = default);

    Task<CommandResult> Restore(GitRestoreOptions options, CancellationToken token = default);

    Task<CommandResult> Revert(GitRevertOptions options, CancellationToken token = default);

    Task<CommandResult> RevList(GitRevListOptions options, CancellationToken token = default);

    Task<CommandResult> RevParse(GitRevParseOptions options, CancellationToken token = default);

    Task<CommandResult> Rm(GitRmOptions options, CancellationToken token = default);

    Task<CommandResult> SendEmail(GitSendEmailOptions options, CancellationToken token = default);

    Task<CommandResult> Shortlog(GitShortlogOptions options, CancellationToken token = default);

    Task<CommandResult> Show(GitShowOptions options, CancellationToken token = default);

    Task<CommandResult> ShowRef(GitShowRefOptions options, CancellationToken token = default);

    Task<CommandResult> Stash(GitStashOptions options, CancellationToken token = default);

    Task<CommandResult> Status(GitStatusOptions options, CancellationToken token = default);

    Task<CommandResult> Submodule(GitSubmoduleOptions options, CancellationToken token = default);

    Task<CommandResult> Svn(GitSvnOptions options, CancellationToken token = default);

    Task<CommandResult> Switch(GitSwitchOptions options, CancellationToken token = default);

    Task<CommandResult> SymbolicRef(GitSymbolicRefOptions options, CancellationToken token = default);

    Task<CommandResult> Tag(GitTagOptions options, CancellationToken token = default);

    Task<CommandResult> UpdateIndex(GitUpdateIndexOptions options, CancellationToken token = default);

    Task<CommandResult> UpdateRef(GitUpdateRefOptions options, CancellationToken token = default);

    Task<CommandResult> UpdateServerInfo(GitUpdateServerInfoOptions options, CancellationToken token = default);

    Task<CommandResult> VerifyPack(GitVerifyPackOptions options, CancellationToken token = default);

    Task<CommandResult> Worktree(GitWorktreeOptions options, CancellationToken token = default);

    Task<CommandResult> WriteTree(GitWriteTreeOptions options, CancellationToken token = default);
}