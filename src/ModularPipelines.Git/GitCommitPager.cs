using System.Runtime.CompilerServices;
using ModularPipelines.Exceptions;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal static class GitCommitPager
{
    private const int PageSize = 50;
    private const char RecordSeparator = '\0';

    public static async IAsyncEnumerable<GitCommit> EnumerateAsync(
        IGitCommandRunner gitCommandRunner,
        IGitCommitMapper gitCommitMapper,
        string? branch,
        GitOptions? options,
        CommandExecutionOptions? commandExecutionOptions,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var completeOutputOptions = GetCompleteOutputOptions(commandExecutionOptions);
        var skip = 0;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var output = await GetPageOutputAsync(
                gitCommandRunner,
                options,
                completeOutputOptions,
                branch,
                skip,
                cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            var records = output
                .Split(RecordSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(record => record.Trim('\r', '\n'))
                .Where(record => !string.IsNullOrWhiteSpace(record))
                .ToArray();
            foreach (var record in records)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return gitCommitMapper.Map(record);
            }

            if (records.Length < PageSize)
            {
                yield break;
            }

            skip += records.Length;
        }
    }

    private static async Task<string> GetPageOutputAsync(
        IGitCommandRunner gitCommandRunner,
        GitOptions? options,
        CommandExecutionOptions executionOptions,
        string? branch,
        int skip,
        CancellationToken cancellationToken)
    {
        string?[] arguments =
        [
            branch,
            $"--skip={skip}",
            $"--max-count={PageSize}",
            $"--format={GitConstants.CommitLogFormat}%x00",
        ];
        try
        {
            return await RunCommandAsync(
                gitCommandRunner, options, executionOptions,
                "log", arguments, cancellationToken).ConfigureAwait(false);
        }
        catch (CommandException exception) when (exception.Result.ExitCode == 128 &&
                                                branch is null && skip == 0)
        {
            // An unborn HEAD makes the default log fail. Check all refs as well as HEAD:
            // an invalid revision or an unreadable repository must not become empty history.
            var repositoryOptions = options is null ? null : options with
            {
                AdditionalArguments = options.AdditionalArguments?.Where(argument => argument.IsGlobalOption).ToArray(),
                RunSettings = null,
            };
            var revisions = await RunCommandAsync(
                gitCommandRunner, repositoryOptions, executionOptions,
                "rev-parse", ["--revs-only", "--all", "HEAD"], cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(revisions))
            {
                throw;
            }

            // With no revisions, --all produces a successful empty log. Retain the
            // original log settings so invalid options and other failures still throw.
            return await RunCommandAsync(
                gitCommandRunner, options, executionOptions,
                "log", [.. arguments, "--all"], cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task<string> RunCommandAsync(
        IGitCommandRunner gitCommandRunner,
        GitOptions? options,
        CommandExecutionOptions executionOptions,
        string command,
        string?[] arguments,
        CancellationToken cancellationToken)
    {
        if (options is null)
        {
            return await gitCommandRunner.RunCommandsAsync(
                executionOptions, cancellationToken, [command, .. arguments]).ConfigureAwait(false);
        }

        var pageOptions = options with
        {
            CommandParts = [command],
            Arguments = [.. arguments.OfType<string>()],
        };
        return await gitCommandRunner.RunCommandsAsync(
            pageOptions, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    internal static CommandExecutionOptions GetCompleteOutputOptions(
        CommandExecutionOptions? commandExecutionOptions) =>
        (commandExecutionOptions ?? new CommandExecutionOptions()) with
        {
            MaxCapturedOutputLength = 0,
            ThrowOnNonZeroExitCode = true,
        };
}
