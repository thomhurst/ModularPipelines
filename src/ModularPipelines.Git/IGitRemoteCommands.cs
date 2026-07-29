using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public interface IGitRemoteCommands
{
    Task<CommandResult> FetchAsync(GitFetchOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> PullAsync(GitPullOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> PushAsync(GitPushOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RemoteAsync(GitRemoteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> RequestPullAsync(GitRequestPullOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SendEmailAsync(GitSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SubmoduleAsync(GitSubmoduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);

    Task<CommandResult> SvnAsync(GitSvnOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);
}
