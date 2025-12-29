using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation, IInitializer
{
    private readonly ILogger<GitInformation> _logger;
    private readonly ICommand _command;
    private readonly IGitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitInformation(
        ILogger<GitInformation> logger,
        ICommand command,
        IGitCommandRunner gitCommandRunner,
        IGitCommitMapper gitCommitMapper)
    {
        _logger = logger;
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    public Folder Root { get; private set; } = null!;
    public string BranchName { get; private set; } = "";
    public string DefaultBranchName { get; private set; } = "";
    public string LastCommitSha { get; private set; } = "";
    public string LastCommitShortSha { get; private set; } = "";
    public string Tag { get; private set; } = "";
    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
    public GitCommit? PreviousCommit { get; private set; }

    public async Task InitializeAsync()
    {
        await VerifyGitAvailable();

        var tasks = new List<Task>();

        Async(async () =>
        {
            var root = await GetOutput(new GitRevParseOptions { ShowToplevel = true });
            Root = root != null ? new Folder(root) : throw new InvalidOperationException("Not a git repository");
        });

        Async(async () => BranchName = await GetOutput(new GitBranchOptions { ShowCurrent = true }) ?? "");

        Async(async () => DefaultBranchName = await GetDefaultBranchName() ?? "");

        Async(async () => LastCommitSha = await GetOutput(new GitRevParseOptions { Arguments = ["HEAD"] }) ?? "");

        Async(async () => LastCommitShortSha = await GetOutput(new GitRevParseOptions { Short = true, Arguments = ["HEAD"] }) ?? "");

        Async(async () => Tag = await GetOutput(new GitDescribeOptions { Tags = true }) ?? "");

        Async(async () =>
        {
            var countStr = await GetOutput(new GitRevListOptions { Count = true, Arguments = ["HEAD"] });
            CommitsOnBranch = int.TryParse(countStr, out var count) ? count : 0;
        });

        Async(async () =>
        {
            var timestampStr = await GetOutput(new GitLogOptions { Format = "%at", Arguments = ["-1"] });
            LastCommitDateTime = long.TryParse(timestampStr, out var timestamp)
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
                : DateTimeOffset.MinValue;
        });

        Async(async () => PreviousCommit = await GetPreviousCommit());

        await Task.WhenAll(tasks);
        return;

        void Async(Func<Task> task) => tasks.Add(task());
    }

    private async Task VerifyGitAvailable()
    {
        try
        {
            await _command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = ["version"],
                LogSettings = CommandLoggingOptions.Silent,
            });
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Git is not available. Ensure git is installed and in PATH.", e);
        }
    }

    private async Task<string?> GetDefaultBranchName()
    {
        try
        {
            var output = await GetOutput(new GitRemoteOptions { Arguments = ["show", "origin"] });
            if (output == null)
            {
                return null;
            }

            return output.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("HEAD branch:"))
                ?.Split("HEAD branch: ")[1];
        }
        catch
        {
            var output = await GetOutput(new GitRevParseOptions
            {
                Arguments = ["origin/HEAD"],
                AbbrevRef = true,
                ThrowOnNonZeroExitCode = false,
            });

            return output?.Replace("origin/", string.Empty);
        }
    }

    private async Task<string?> GetOutput(GitOptions gitOptions)
    {
        try
        {
            var result = await _command.ExecuteCommandLineTool(gitOptions with
            {
                LogSettings = _logger.IsEnabled(LogLevel.Debug) ? CommandLoggingOptions.Diagnostic : CommandLoggingOptions.Silent,
            });
            return result.StandardOutput.Trim();
        }
        catch (Exception exception)
        {
            _logger.LogDebug(exception, "Error running Git command");
            return null;
        }
    }

    private async Task<GitCommit?> GetPreviousCommit()
    {
        var output = await _gitCommandRunner.RunCommandsOrNull(null, "log", "--skip=0", "-1",
            $"--format={GitConstants.CommitLogFormat}");

        return string.IsNullOrWhiteSpace(output) ? null : _gitCommitMapper.Map(output);
    }

    public IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    public async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var index = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var output = await _gitCommandRunner.RunCommandsOrNull(options, "log", branch, $"--skip={index}", "-1", $"--format={GitConstants.CommitLogFormat}");

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }
}
