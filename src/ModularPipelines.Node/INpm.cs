using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
    Task<CommandResult> Install(NpmInstallOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default);
}
