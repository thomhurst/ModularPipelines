using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitHistoryCommands
{
    Task<CommandResult> BlameAsync(GitBlameOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> DescribeAsync(GitDescribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ForEachRefAsync(GitForEachRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FormatPatchAsync(GitFormatPatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> LogAsync(GitLogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> NotesAsync(GitNotesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RangeDiffAsync(GitRangeDiffOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ReflogAsync(GitReflogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RevListAsync(GitRevListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RevParseAsync(GitRevParseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShortlogAsync(GitShortlogOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShowAsync(GitShowOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> ShowRefAsync(GitShowRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SymbolicRefAsync(GitSymbolicRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> CommitsAsync(GitOptions? options = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> CommitsAsync(string? branch, GitOptions? options = null, CancellationToken cancellationToken = default);
}
