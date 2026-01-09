using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

[ExcludeFromCodeCoverage]
public class GitCommands : IGitCommands
{
    private readonly ICommand _command;
    private readonly IGitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitCommands(ICommand command, IGitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Add(GitAddOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Am(GitAmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Apply(GitApplyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Archive(GitArchiveOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Base(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bisect(GitBisectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Blame(GitBlameOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Branch(GitBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bugreport(GitBugreportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Bundle(GitBundleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CatFile(GitCatFileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckIgnore(GitCheckIgnoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckoutIndex(GitCheckoutIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Checkout(GitCheckoutOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CherryPick(GitCherryPickOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clean(GitCleanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clone(GitCloneOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Commit(GitCommitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CommitTree(GitCommitTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Config(GitConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CountObjects(GitCountObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Daemon(GitDaemonOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Describe(GitDescribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DiffIndex(GitDiffIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Diff(GitDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Difftool(GitDifftoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FastImport(GitFastImportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fetch(GitFetchOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitFetchOptions(), executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FilterBranch(GitFilterBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ForEachRef(GitForEachRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> FormatPatch(GitFormatPatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fsck(GitFsckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Gc(GitGcOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitGcOptions(), executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Grep(GitGrepOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> HashObject(GitHashObjectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Help(GitHelpOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Init(GitInitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Instaweb(GitInstawebOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Log(GitLogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsFiles(GitLsFilesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> LsTree(GitLsTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> MergeBase(GitMergeBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Merge(GitMergeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Mergetool(GitMergetoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Mv(GitMvOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Notes(GitNotesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Git(GitBaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Pull(GitPullOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitPullOptions(), executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Push(GitPushOptions? options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GitPushOptions(), executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RangeDiff(GitRangeDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ReadTree(GitReadTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Rebase(GitRebaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Reflog(GitReflogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Remote(GitRemoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RequestPull(GitRequestPullOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Reset(GitResetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Restore(GitRestoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Revert(GitRevertOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevList(GitRevListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RevParse(GitRevParseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Rm(GitRmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SendEmail(GitSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Shortlog(GitShortlogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Show(GitShowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ShowRef(GitShowRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Stash(GitStashOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Status(GitStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Submodule(GitSubmoduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Svn(GitSvnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Switch(GitSwitchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SymbolicRef(GitSymbolicRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Tag(GitTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateIndex(GitUpdateIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateRef(GitUpdateRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateServerInfo(GitUpdateServerInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> VerifyPack(GitVerifyPackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Worktree(GitWorktreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WriteTree(GitWriteTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token).ConfigureAwait(false);
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
            var output = await _gitCommandRunner.RunCommandsOrNull(null, "log", branch, $"--skip={index}", "-1", $"--format={GitConstants.CommitLogFormat}").ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }
}
