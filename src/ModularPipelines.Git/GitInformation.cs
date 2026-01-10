using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation, IInitializer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitInformation(
        IServiceScopeFactory serviceScopeFactory,
        IGitCommitMapper gitCommitMapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
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
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GitInformation>>();
        var command = scope.ServiceProvider.GetRequiredService<ICommand>();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<IGitCommandRunner>();

        await VerifyGitAvailable(command).ConfigureAwait(false);

        var tasks = new List<Task>();

        Async(async () =>
        {
            var root = await GetOutput(command, logger, new GitRevParseOptions { ShowToplevel = true }).ConfigureAwait(false);
            Root = root != null ? new Folder(root) : throw new InvalidOperationException("Not a git repository");
        });

        Async(async () => BranchName = await GetOutput(command, logger, new GitBranchOptions { ShowCurrent = true }).ConfigureAwait(false) ?? "");

        Async(async () => DefaultBranchName = await GetDefaultBranchName(command, logger).ConfigureAwait(false) ?? "");

        Async(async () => LastCommitSha = await GetOutput(command, logger, new GitRevParseOptions { Arguments = ["HEAD"] }).ConfigureAwait(false) ?? "");

        Async(async () => LastCommitShortSha = await GetOutput(command, logger, new GitRevParseOptions { Short = true, Arguments = ["HEAD"] }).ConfigureAwait(false) ?? "");

        Async(async () => Tag = await GetOutput(command, logger, new GitDescribeOptions { Tags = true }).ConfigureAwait(false) ?? "");

        Async(async () =>
        {
            var countStr = await GetOutput(command, logger, new GitRevListOptions { Count = true, Arguments = ["HEAD"] }).ConfigureAwait(false);
            CommitsOnBranch = int.TryParse(countStr, out var count) ? count : 0;
        });

        Async(async () =>
        {
            var timestampStr = await GetOutput(command, logger, new GitLogOptions { Format = GitConstants.AuthorTimestampFormat, Arguments = ["-1"] }).ConfigureAwait(false);
            LastCommitDateTime = long.TryParse(timestampStr, out var timestamp)
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
                : DateTimeOffset.MinValue;
        });

        Async(async () => PreviousCommit = await GetPreviousCommit(gitCommandRunner).ConfigureAwait(false));

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return;

        void Async(Func<Task> task) => tasks.Add(task());
    }

    private static async Task VerifyGitAvailable(ICommand command)
    {
        try
        {
            await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("git")
                {
                    Arguments = ["version"],
                },
                new CommandExecutionOptions
                {
                    LogSettings = CommandLoggingOptions.Silent,
                }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Git is not available. Ensure git is installed and in PATH.", e);
        }
    }

    private static async Task<string?> GetDefaultBranchName(ICommand command, ILogger logger)
    {
        try
        {
            var output = await GetOutput(command, logger, new GitRemoteOptions { Arguments = ["show", "origin"] }).ConfigureAwait(false);
            if (output == null)
            {
                return null;
            }

            return output.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("HEAD branch:"))
                ?.Split("HEAD branch: ")[1];
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            // Fallback: If 'git remote show origin' fails (e.g., no remote configured, network issues,
            // or shallow clone), try parsing origin/HEAD reference instead. This provides a best-effort
            // approach to determine the default branch in various git repository states.
            logger.LogDebug(ex, "Failed to get default branch from 'git remote show origin', falling back to origin/HEAD");
            var output = await GetOutput(command, logger, new GitRevParseOptions
            {
                Arguments = ["origin/HEAD"],
                AbbrevRef = true,
            }, new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = false,
            }).ConfigureAwait(false);

            return output?.Replace("origin/", string.Empty);
        }
    }

    private static async Task<string?> GetOutput(ICommand command, ILogger logger, GitOptions gitOptions, CommandExecutionOptions? executionOptions = null)
    {
        try
        {
            var options = executionOptions ?? new CommandExecutionOptions();
            options = options with
            {
                LogSettings = logger.IsEnabled(LogLevel.Debug) ? CommandLoggingOptions.Diagnostic : CommandLoggingOptions.Silent,
            };
            var result = await command.ExecuteCommandLineTool(gitOptions, options).ConfigureAwait(false);
            return result.StandardOutput.Trim();
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            logger.LogDebug(exception, "Error running Git command");
            return null;
        }
    }

    private async Task<GitCommit?> GetPreviousCommit(IGitCommandRunner gitCommandRunner)
    {
        var output = await gitCommandRunner.RunCommandsOrNull(null, "log", "--skip=0", "-1",
            $"--format={GitConstants.CommitLogFormat}").ConfigureAwait(false);

        return string.IsNullOrWhiteSpace(output) ? null : _gitCommitMapper.Map(output);
    }

    public IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    public async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<IGitCommandRunner>();

        var index = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var output = await gitCommandRunner.RunCommandsOrNull(null, "log", branch, $"--skip={index}", "-1", $"--format={GitConstants.CommitLogFormat}").ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }
}
