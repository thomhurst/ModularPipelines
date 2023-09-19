using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Git;

internal class StaticGitInformation : IInitializer
{
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;
    
    public static StaticGitInformation? Instance { get; private set; }
    public static readonly object _lock = new();

    public StaticGitInformation(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateAsyncScope();
        _gitCommandRunner = scope.ServiceProvider.GetRequiredService<GitCommandRunner>();
        _gitCommitMapper = scope.ServiceProvider.GetRequiredService<IGitCommitMapper>();
    }

    public async Task InitializeAsync()
    {
        lock (_lock)
        {
            if (Instance != null)
            {
                Root = Instance.Root!;
                BranchName = Instance.BranchName!;
                DefaultBranchName = Instance.DefaultBranchName!;
                LastCommitSha = Instance.LastCommitSha!;
                LastCommitShortSha = Instance.LastCommitShortSha!;
                Tag = Instance.Tag!;
                CommitsOnBranch = Instance.CommitsOnBranch!;
                LastCommitDateTime = Instance.LastCommitDateTime!;
                PreviousCommit = Instance.PreviousCommit!;
                return;
            }

            Instance = this;
        }

        try
        {
            await _gitCommandRunner.RunCommands(null, "version");
        }
        catch (Exception e)
        {
            throw new Exception("Error detecting Git repository", e);
        }

        var tasks = new List<Task>();
        
        Async(async () => 
            Root = (await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--show-toplevel"))!
        );

        Async(async () =>
            BranchName = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--abbrev-ref", "HEAD")
        );

        Async(async () =>
            DefaultBranchName =
                (await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--abbrev-ref", "origin/HEAD"))?.Replace(
                    "origin/", string.Empty)
        );

        Async(async () =>
            LastCommitSha = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "HEAD")
        );

        Async(async () =>
            LastCommitShortSha = await _gitCommandRunner.RunCommandsOrNull(null, "rev-parse", "--short", "HEAD")
        );

        Async(async () =>
            Tag = await _gitCommandRunner.RunCommandsOrNull(null, "describe", "--tags")
        );

        Async(async () =>
            CommitsOnBranch =
                int.Parse(await _gitCommandRunner.RunCommandsOrNull(null, "rev-list", "HEAD", "--count") ?? "0")
        );

        Async(async () =>
            LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(
                long.Parse(await _gitCommandRunner.RunCommandsOrNull(null, "log", "-1", "--format=%at") ?? "0"))
        );

        Async(async () =>
            PreviousCommit = await LastCommits(1).FirstOrDefaultAsync()
        );
        
        await Task.WhenAll(tasks);
        return;

        void Async(Func<Task> task)
        {
            tasks.Add(task());
        }
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
}
