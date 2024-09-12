using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation
{
    private readonly StaticGitInformation _staticGitInformation;
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitInformation(StaticGitInformation staticGitInformation,
        GitCommandRunner gitCommandRunner,
        IGitCommitMapper gitCommitMapper,
        IEnvironmentContext environmentContext)
    {
        _staticGitInformation = staticGitInformation;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    public Folder Root => _staticGitInformation.Root.AssertExists();

    public string? BranchName => _staticGitInformation.BranchName;

    public string? DefaultBranchName => _staticGitInformation.DefaultBranchName;

    public string? Tag => _staticGitInformation.Tag;

    public GitCommit? PreviousCommit => _staticGitInformation.PreviousCommit;

    public int CommitsOnBranch => _staticGitInformation.CommitsOnBranch ?? 0;

    public DateTimeOffset LastCommitDateTime => _staticGitInformation.LastCommitDateTime ?? DateTimeOffset.MinValue;

    public string? LastCommitSha => _staticGitInformation.LastCommitSha;

    public string? LastCommitShortSha => _staticGitInformation.LastCommitShortSha;

    public IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    public async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var index = 0;
        while (true)
        {
            var output = await _gitCommandRunner.RunCommandsOrNull(options, "log", branch, $"--skip={index - 1}", "-1", $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %B'");

            index++;

            if (string.IsNullOrWhiteSpace(output) || cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return _gitCommitMapper.Map(output);
        }
    }
}