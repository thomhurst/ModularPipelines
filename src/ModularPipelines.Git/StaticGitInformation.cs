using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Git;

internal class StaticGitInformation : IGitInformation, IInitializer
{
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;
    
    public static StaticGitInformation? Instance { get; private set; }

    public StaticGitInformation(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateAsyncScope();
        _gitCommandRunner = scope.ServiceProvider.GetRequiredService<GitCommandRunner>();
        _gitCommitMapper = scope.ServiceProvider.GetRequiredService<IGitCommitMapper>();;
    }

    public async Task InitializeAsync()
    {
        Instance ??= this;
        
        try
        {
            await _gitCommandRunner.RunCommands(null, "version");
        }
        catch (Exception e)
        {
            throw new Exception("Error detecting Git repository", e);
        }

        Root = (await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--show-toplevel"))!;
        BranchName = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--abbrev-ref", "HEAD");
        DefaultBranchName = (await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--abbrev-ref", "origin/HEAD"))?.Replace("origin/", string.Empty);
        LastCommitSha = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "HEAD");
        LastCommitShortSha = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--short", "HEAD");
        Tag = await _gitCommandRunner.RunCommandsOrNull(null, "describe", "--tags");
        CommitsOnBranch = int.Parse(await _gitCommandRunner.RunCommandsOrNull(null, "rev-list", "HEAD", "--count") ?? "0");
        LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(await _gitCommandRunner.RunCommandsOrNull(null, "log", "-1", "--format=%at") ?? "0"));
        PreviousCommit = await LastCommits(1).FirstOrDefaultAsync();
    }

    public async IAsyncEnumerable<GitCommit> LastCommits(int count, GitOptions? gitOptions = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < count; i++)
        {
            var output = await _gitCommandRunner.RunCommandsOrNull(gitOptions, "log", $"--skip={i - 1}", "-1", $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %b'");

            if (string.IsNullOrWhiteSpace(output) || cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return _gitCommitMapper.Map(output);
        }
    }

    public GitCommit? PreviousCommit { get; private set; }

    public Folder Root { get; private set; } = null!;
    public string? BranchName { get; private set; } = null!;
    public string? DefaultBranchName { get; private set; } = null!;

    public string? Tag { get; private set; } = null!;

    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }

    public string? LastCommitSha { get; private set; } = null!;

    public string? LastCommitShortSha { get; private set; } = null!;

    public IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<GitCommit> Commits(string branch, GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
