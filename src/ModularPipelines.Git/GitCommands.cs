using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Git;

[ExcludeFromCodeCoverage]
public class GitCommands : IGitCommands
{
    private readonly ICommand _command;
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitCommands(ICommand command, GitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Add(GitAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Am(GitAmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Apply(GitApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Archive(GitArchiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Base(GitBaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bisect(GitBisectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Blame(GitBlameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Branch(GitBranchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bugreport(GitBugreportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bundle(GitBundleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CatFile(GitCatFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckIgnore(GitCheckIgnoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckoutIndex(GitCheckoutIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Checkout(GitCheckoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CherryPick(GitCherryPickOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clean(GitCleanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clone(GitCloneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Commit(GitCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CommitTree(GitCommitTreeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Config(GitConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CountObjects(GitCountObjectsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Daemon(GitDaemonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Describe(GitDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DiffIndex(GitDiffIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Diff(GitDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Difftool(GitDifftoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FastImport(GitFastImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fetch(GitFetchOptions? options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitFetchOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FilterBranch(GitFilterBranchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ForEachRef(GitForEachRefOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FormatPatch(GitFormatPatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fsck(GitFsckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Gc(GitGcOptions? options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitGcOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Grep(GitGrepOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> HashObject(GitHashObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Help(GitHelpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Init(GitInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Instaweb(GitInstawebOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Log(GitLogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsFiles(GitLsFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsTree(GitLsTreeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MergeBase(GitMergeBaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Merge(GitMergeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Mergetool(GitMergetoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Mv(GitMvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Notes(GitNotesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Git(GitBaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Pull(GitPullOptions? options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitPullOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Push(GitPushOptions? options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitPushOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RangeDiff(GitRangeDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ReadTree(GitReadTreeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Rebase(GitRebaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Reflog(GitReflogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Remote(GitRemoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RequestPull(GitRequestPullOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Reset(GitResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Restore(GitRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Revert(GitRevertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevList(GitRevListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevParse(GitRevParseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Rm(GitRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SendEmail(GitSendEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Shortlog(GitShortlogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Show(GitShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ShowRef(GitShowRefOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Stash(GitStashOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Status(GitStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Submodule(GitSubmoduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Svn(GitSvnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Switch(GitSwitchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SymbolicRef(GitSymbolicRefOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Tag(GitTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateIndex(GitUpdateIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateRef(GitUpdateRefOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateServerInfo(GitUpdateServerInfoOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> VerifyPack(GitVerifyPackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Worktree(GitWorktreeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WriteTree(GitWriteTreeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var index = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var output = await _gitCommandRunner.RunCommandsOrNull(options, "log", branch, $"--skip={index}", "-1", $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %B'");

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }
}
