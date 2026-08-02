using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IGitCommitMapper _gitCommitMapper;
    private readonly SemaphoreSlim _repositoryInfoLock = new(1, 1);
    private GitRepositoryInfo? _repositoryInfo;
    private bool _repositoryInfoLoaded;

    public GitInformation(
        IServiceScopeFactory serviceScopeFactory,
        IGitCommitMapper gitCommitMapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _gitCommitMapper = gitCommitMapper;
    }

    public async Task<GitRepositoryInfo?> GetInfoAsync(CancellationToken cancellationToken = default)
    {
        if (Volatile.Read(ref _repositoryInfoLoaded))
        {
            return _repositoryInfo;
        }

        await _repositoryInfoLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!_repositoryInfoLoaded)
            {
                _repositoryInfo = await LoadInfoAsync(cancellationToken).ConfigureAwait(false);
                Volatile.Write(ref _repositoryInfoLoaded, true);
            }

            return _repositoryInfo;
        }
        finally
        {
            _repositoryInfoLock.Release();
        }
    }

    public IAsyncEnumerable<GitCommit> Commits(
        GitOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    public async IAsyncEnumerable<GitCommit> Commits(
        string? branch,
        GitOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<IGitCommandRunner>();

        var index = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var output = await gitCommandRunner.RunCommandsOrNull(
                null,
                cancellationToken,
                "log",
                branch,
                $"--skip={index}",
                "-1",
                $"--format={GitConstants.CommitLogFormat}").ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }

    private async Task<GitRepositoryInfo?> LoadInfoAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GitInformation>>();
        var command = scope.ServiceProvider.GetRequiredService<ICommandContext>();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<IGitCommandRunner>();
        var root = await GetOutput(
            command,
            logger,
            new GitRevParseOptions { ShowToplevel = true },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(root))
        {
            return null;
        }

        var branchName = GetOutput(
            command,
            logger,
            new GitBranchOptions { ShowCurrent = true },
            cancellationToken: cancellationToken);
        var defaultBranchName = GetDefaultBranchName(command, logger, cancellationToken);
        var lastCommitSha = GetOutput(
            command,
            logger,
            new GitRevParseOptions { Committish = "HEAD" },
            cancellationToken: cancellationToken);
        var lastCommitShortSha = GetOutput(
            command,
            logger,
            new GitRevParseOptions { Short = true, Committish = "HEAD" },
            cancellationToken: cancellationToken);
        var tag = GetOutput(
            command,
            logger,
            new GitDescribeOptions { Tags = true },
            cancellationToken: cancellationToken);
        var commitCount = GetOutput(
            command,
            logger,
            new GitRevListOptions { Count = true, Ref = "HEAD" },
            cancellationToken: cancellationToken);
        var lastCommitTimestamp = GetOutput(
            command,
            logger,
            new GitLogOptions { Format = GitConstants.AuthorTimestampFormat, MaxCount = "1" },
            cancellationToken: cancellationToken);
        var previousCommit = GetPreviousCommit(gitCommandRunner, cancellationToken);

        await Task.WhenAll(
            branchName,
            defaultBranchName,
            lastCommitSha,
            lastCommitShortSha,
            tag,
            commitCount,
            lastCommitTimestamp,
            previousCommit).ConfigureAwait(false);

        return new GitRepositoryInfo(new Folder(root))
        {
            BranchName = NullIfEmpty(await branchName.ConfigureAwait(false)),
            DefaultBranchName = NullIfEmpty(await defaultBranchName.ConfigureAwait(false)),
            LastCommitSha = NullIfEmpty(await lastCommitSha.ConfigureAwait(false)),
            LastCommitShortSha = NullIfEmpty(await lastCommitShortSha.ConfigureAwait(false)),
            Tag = NullIfEmpty(await tag.ConfigureAwait(false)),
            CommitsOnBranch = ParseInt32(await commitCount.ConfigureAwait(false)),
            LastCommitDateTime = ParseTimestamp(await lastCommitTimestamp.ConfigureAwait(false)),
            PreviousCommit = await previousCommit.ConfigureAwait(false),
        };
    }

    private static async Task<string?> GetDefaultBranchName(
        ICommandContext command,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        try
        {
            var output = await GetOutput(
                command,
                logger,
                new GitRemoteShowOptions { Remote = "origin" },
                cancellationToken: cancellationToken).ConfigureAwait(false);
            if (output == null)
            {
                return null;
            }

            return output.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("HEAD branch:"))
                ?.Split("HEAD branch: ")[1];
        }
        catch (Exception ex) when (ex is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            // Fallback: If 'git remote show origin' fails (e.g., no remote configured, network issues,
            // or shallow clone), try parsing origin/HEAD reference instead. This provides a best-effort
            // approach to determine the default branch in various git repository states.
            logger.LogDebug(ex, "Failed to get default branch from 'git remote show origin', falling back to origin/HEAD");
            var output = await GetOutput(command, logger, new GitRevParseOptions
            {
                Committish = "origin/HEAD",
                AbbrevRef = true,
            }, new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = false,
            }, cancellationToken).ConfigureAwait(false);

            return output?.Replace("origin/", string.Empty);
        }
    }

    private static async Task<string?> GetOutput(
        ICommandContext command,
        ILogger logger,
        GitOptions gitOptions,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = executionOptions ?? new CommandExecutionOptions();
            options = options with
            {
                // Always use Silent logging for git initialization commands
                // These are internal one-time setup commands that don't need to be logged
                LogSettings = CommandLoggingOptions.Silent,
            };
            var result = await command.ExecuteCommandLineToolAsync(gitOptions, options, cancellationToken).ConfigureAwait(false);
            return result.StandardOutput.Trim();
        }
        catch (Exception exception) when (exception is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            logger.LogDebug(exception, "Error running Git command");
            return null;
        }
    }

    private async Task<GitCommit?> GetPreviousCommit(
        IGitCommandRunner gitCommandRunner,
        CancellationToken cancellationToken)
    {
        var output = await gitCommandRunner.RunCommandsOrNull(null, cancellationToken, "log", "--skip=0", "-1",
            $"--format={GitConstants.CommitLogFormat}").ConfigureAwait(false);

        return string.IsNullOrWhiteSpace(output) ? null : _gitCommitMapper.Map(output);
    }

    private static string? NullIfEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static int? ParseInt32(string? value)
    {
        return int.TryParse(value, out var result) ? result : null;
    }

    private static DateTimeOffset? ParseTimestamp(string? value)
    {
        return long.TryParse(value, out var timestamp)
            ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
            : null;
    }
}
