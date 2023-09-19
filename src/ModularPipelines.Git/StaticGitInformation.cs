using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Git;

internal class StaticGitInformation : IInitializer
{
    private readonly ILogger<StaticGitInformation> _logger;
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;
    
    public static StaticGitInformation? Instance { get; private set; }
    public static readonly object _lock = new();
    private readonly ICommand _command;

    public StaticGitInformation(IServiceProvider serviceProvider,
        ILogger<StaticGitInformation> logger)
    {
        _logger = logger;
        var scope = serviceProvider.CreateAsyncScope();
        _command = scope.ServiceProvider.GetRequiredService<ICommand>();
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
            await _command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = new[] { "version" }
            });
        }
        catch (Exception e)
        {
            throw new Exception("Error detecting Git repository", e);
        }

        var tasks = new List<Task>();

        Async(async () =>
            Root = (await GetOutput(new GitRevParseOptions
            {
                ShowToplevel = true
            }))!);

        Async(async () =>
            BranchName = await GetOutput(new GitRevParseOptions
            {
                AbbrevRef = true,
                Arguments = new []{ "HEAD" }
            })
        );

        Async(async () =>
        {
            var output = await GetOutput(new GitSymbolicRefOptions
            {
                Arguments = new[] { "refs/remotes/origin/HEAD" },
                Short = true
            });

            DefaultBranchName = output?.Replace("origin/", string.Empty);
        });

        Async(async () =>
            LastCommitSha = await GetOutput(new GitRevParseOptions
            {
                Arguments = new[] { "HEAD" }
            })
        );

        Async(async () =>
            LastCommitShortSha = await GetOutput(new GitRevParseOptions
            {
                Short = true,
                Arguments = new []{ "HEAD" }
            })
        );

        Async(async () =>
            Tag = await GetOutput(new GitDescribeOptions
            {
                Tags = true
            })
        );

        Async(async () =>
            CommitsOnBranch =
                int.Parse(await GetOutput(new GitRevListOptions
                {
                    Count = true,
                    Arguments = new []{ "HEAD" }
                }) ?? "0")
        );

        Async(async () =>
            LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(
                long.Parse(await GetOutput(new GitLogOptions
                {
                    Format = "%at",
                    Arguments = new []{ "-1" }
                }) ?? "0"))
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

    private async Task<string?> GetOutput(GitOptions gitOptions)
    {
        try
        {
            var result = await _command.ExecuteCommandLineTool(gitOptions);
            return result.StandardOutput;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error running Git command");
            return null;
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
