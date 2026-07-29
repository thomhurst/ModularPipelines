using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitMaintenanceCommands
{
    Task<CommandResult> CatFileAsync(GitCatFileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CountObjectsAsync(GitCountObjectsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FastImportAsync(GitFastImportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FilterBranchAsync(GitFilterBranchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> FsckAsync(GitFsckOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> GcAsync(GitGcOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> HashObjectAsync(GitHashObjectOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> LsTreeAsync(GitLsTreeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateRefAsync(GitUpdateRefOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateServerInfoAsync(GitUpdateServerInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> VerifyPackAsync(GitVerifyPackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}
