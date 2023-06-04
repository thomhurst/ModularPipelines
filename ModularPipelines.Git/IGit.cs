using CliWrap.Buffered;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGit
{
    Task<BufferedCommandResult> Checkout(GitCheckoutOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Version(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Fetch(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Pull(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Push(GitOptions? options = null, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> CustomCommand(GitCommandOptions options, CancellationToken cancellationToken);
}

public interface IGit<T> : IGit
{
}