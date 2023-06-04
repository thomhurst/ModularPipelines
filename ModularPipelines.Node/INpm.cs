using CliWrap.Buffered;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm<T> : INpm
{
}

public interface INpm
{
    Task<BufferedCommandResult> Install(NpmInstallOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default);
}