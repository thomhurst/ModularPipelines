using CliWrap.Buffered;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet;

public interface IDotNet
{
    Task<BufferedCommandResult> Restore(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Build(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Publish(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Pack(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Clean(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Test(DotNetOptions options, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> Version(CommandEnvironmentOptions? options = null, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default);
}