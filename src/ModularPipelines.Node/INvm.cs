using ModularPipelines.Models;

namespace ModularPipelines.Node;

public interface INvm
{
    Task<CommandResult> UseAsync(string version, CancellationToken cancellationToken = default);

    Task<CommandResult> InstallAsync(string version, CancellationToken cancellationToken = default);

    Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default);

    Task<CommandResult> WhichAsync(CancellationToken cancellationToken = default);
}