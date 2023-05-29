using CliWrap.Buffered;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGit
{
    Task<BufferedCommandResult> Checkout(GitCheckoutOptions options);
    Task<BufferedCommandResult> Version(GitOptions? options = null);
    Task<BufferedCommandResult> Fetch(GitOptions? options = null);
    Task<BufferedCommandResult> Pull(GitOptions? options = null);
    Task<BufferedCommandResult> Push(GitOptions? options = null);

}