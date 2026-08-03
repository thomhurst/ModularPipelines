using System.Runtime.CompilerServices;
using ModularPipelines.Git.Models;
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
        CommandExecutionOptions? commandExecutionOptions,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var completeOutputOptions = GetCompleteOutputOptions(commandExecutionOptions);
        var skip = 0;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var output = await gitCommandRunner.RunCommandsOrNull(
                completeOutputOptions,
                cancellationToken,
                "log",
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

    internal static CommandExecutionOptions GetCompleteOutputOptions(
        CommandExecutionOptions? commandExecutionOptions) =>
        (commandExecutionOptions ?? new CommandExecutionOptions()) with
        {
            MaxCapturedOutputLength = 0,
        };
}
