using System.Runtime.CompilerServices;
using ModularPipelines.Context;
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
        ICommandContext command,
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
                command,
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
        ICommandContext command,
        GitOptions? options,
        CommandExecutionOptions executionOptions,
        CancellationToken cancellationToken,
        params string?[] arguments)
    {
        if (options is null)
        {
            // Preserve custom low-level runners for calls without typed options.
            return await gitCommandRunner.RunCommandsOrNull(
                executionOptions, cancellationToken, ["log", .. arguments]).ConfigureAwait(false);
        }

        try
        {
            var pageOptions = options with
            {
                CommandParts = ["log"],
                Arguments = [.. arguments.OfType<string>()],
            };
            var result = await command.ExecuteCommandLineToolAsync(
                pageOptions,
                executionOptions with
                {
                    Logging = CommandLoggingOptions.Silent,
                    ThrowOnNonZeroExitCode = true,
                },
                cancellationToken).ConfigureAwait(false);
            return result.StandardOutput;
        }
        catch (Exception exception) when (exception is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            return null;
        }
    }

    internal static CommandExecutionOptions GetCompleteOutputOptions(
        CommandExecutionOptions? commandExecutionOptions) =>
        (commandExecutionOptions ?? new CommandExecutionOptions()) with
        {
            MaxCapturedOutputLength = 0,
        };
}
