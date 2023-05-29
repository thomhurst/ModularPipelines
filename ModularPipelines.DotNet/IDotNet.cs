using CliWrap.Buffered;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet;

public interface IDotNet
{
    Task<BufferedCommandResult> Restore(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Build(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Publish(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Pack(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Clean(DotNetOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Test(DotNetOptions options, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default);
}