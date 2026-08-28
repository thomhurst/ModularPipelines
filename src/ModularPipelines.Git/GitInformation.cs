using ModularPipelines.Context;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    private readonly CommandExecutionOptions? _commandExecutionOptions;
    private readonly SemaphoreSlim _repositoryInfoLock = new(1, 1);
    private GitRepositoryInfo? _repositoryInfo;
    private bool _repositoryInfoLoaded;

    public GitInformation(
        IServiceScopeFactory serviceScopeFactory,
        IGitCommitMapper gitCommitMapper)
        : this(serviceScopeFactory, gitCommitMapper, null)
    {
    }

    internal GitInformation(
        IServiceScopeFactory serviceScopeFactory,
        IGitCommitMapper gitCommitMapper,
        CommandExecutionOptions? commandExecutionOptions)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _gitCommitMapper = gitCommitMapper;
        _commandExecutionOptions = commandExecutionOptions;
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

        await foreach (var commit in GitCommitPager
                           .EnumerateAsync(
                               gitCommandRunner,
                               _gitCommitMapper,
                               branch,
                               _commandExecutionOptions,
                               cancellationToken)
                           .ConfigureAwait(false))
        {
            yield return commit;
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
            _commandExecutionOptions,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(root))
        {
            return null;
        }

        var branchName = GetOutput(
            command,
            logger,
            new GitBranchOptions { ShowCurrent = true },
            _commandExecutionOptions,
            cancellationToken: cancellationToken);
        var defaultBranchName = GetDefaultBranchName(command, logger, _commandExecutionOptions, cancellationToken);
        var lastCommitSha = GetOutput(
            command,
            logger,
            new GitRevParseOptions { Committish = "HEAD" },
            _commandExecutionOptions,
            cancellationToken: cancellationToken);
        var lastCommitShortSha = GetOutput(
            command,
            logger,
            new GitRevParseOptions { Short = true, Committish = "HEAD" },
            _commandExecutionOptions,
            cancellationToken: cancellationToken);
        var tag = GetOutput(
            command,
            logger,
            new GitDescribeOptions { Tags = true },
            _commandExecutionOptions,
            cancellationToken: cancellationToken);
        var commitCount = GetOutput(
            command,
            logger,
            new GitRevListOptions { Count = true, Ref = "HEAD" },
            _commandExecutionOptions,
            cancellationToken: cancellationToken);
        var lastCommitTimestamp = GetOutput(
            command,
            logger,
            new GitLogOptions { Format = GitConstants.AuthorTimestampFormat, MaxCount = "1" },
            _commandExecutionOptions,
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

        return new GitRepositoryInfo(new FolderPath(root))
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
        CommandExecutionOptions? executionOptions,
        CancellationToken cancellationToken)
    {
        var localExecutionOptions = (executionOptions ?? new CommandExecutionOptions()) with
        {
            ThrowOnNonZeroExitCode = false,
        };
        var localOutput = await GetOutput(
            command,
            logger,
            new CommandLineToolOptions("git")
            {
                Arguments = ["symbolic-ref", "--short", "refs/remotes/origin/HEAD"],
            },
            localExecutionOptions,
            cancellationToken).ConfigureAwait(false);
        if (localOutput?.StartsWith("origin/", StringComparison.Ordinal) == true)
        {
            return localOutput["origin/".Length..];
        }

        var remoteExecutionOptions = localExecutionOptions with
        {
            ExecutionTimeout = TimeSpan.FromSeconds(10),
        };
        var remoteOutput = await GetOutput(
            command,
            logger,
            new GitRemoteShowOptions { Remote = "origin" },
            remoteExecutionOptions,
            cancellationToken).ConfigureAwait(false);
        if (remoteOutput == null)
        {
            return null;
        }

        const string headBranchPrefix = "HEAD branch:";
        var headBranch = remoteOutput
            .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(line => line.StartsWith(headBranchPrefix, StringComparison.Ordinal));
        return headBranch?[headBranchPrefix.Length..].Trim();
    }

    private static async Task<string?> GetOutput(
        ICommandContext command,
        ILogger logger,
        CommandLineToolOptions gitOptions,
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
                Logging = CommandLoggingOptions.Silent,
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
        var output = await gitCommandRunner.RunCommandsOrNull(
            GitCommitPager.GetCompleteOutputOptions(_commandExecutionOptions),
            cancellationToken,
            "log",
            "-1",
            $"--format={GitConstants.CommitLogFormat}",
            "HEAD^1").ConfigureAwait(false);

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
