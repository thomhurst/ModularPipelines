using ModularPipelines.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGitOperations
{
    Task<CommandResult> Checkout(GitCheckoutOptions options, CancellationToken cancellationToken = default);
    Task<CommandResult> Version(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> Fetch(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> Pull(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> Push(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> Stage(GitStageOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> Commit(string message, GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<CommandResult> CustomCommand(GitCommandOptions options, CancellationToken cancellationToken);
}