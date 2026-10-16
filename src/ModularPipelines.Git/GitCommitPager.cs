using System.Runtime.CompilerServices;
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
                cancellationToken,
                branch,
                $"--skip={skip}",
                $"--max-count={PageSize}",
                $"--format={GitConstants.CommitLogFormat}%x00").ConfigureAwait(false);
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

    private static async Task<string?> GetPageOutputAsync(
        IGitCommandRunner gitCommandRunner,
        GitOptions? options,
        CommandExecutionOptions executionOptions,
        CancellationToken cancellationToken,
        params string?[] arguments)
    {
        if (options is null)
        {
            return await gitCommandRunner.RunCommandsOrNullAsync(
                executionOptions, cancellationToken, ["log", .. arguments]).ConfigureAwait(false);
        }

        var pageOptions = options with
        {
            CommandParts = ["log"],
            Arguments = [.. arguments.OfType<string>()],
        };
        return await gitCommandRunner.RunCommandsOrNullAsync(
            pageOptions, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    internal static CommandExecutionOptions GetCompleteOutputOptions(
        CommandExecutionOptions? commandExecutionOptions) =>
        (commandExecutionOptions ?? new CommandExecutionOptions()) with
        {
            MaxCapturedOutputLength = 0,
        };
}
