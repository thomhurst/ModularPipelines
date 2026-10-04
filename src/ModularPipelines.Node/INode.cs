using ModularPipelines.Models;
using ModularPipelines.Node.Services;

namespace ModularPipelines.Node;

public interface INode
{
    Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default);

    public INpm Npm { get; }

    public INvm Nvm { get; }

    public INpx Npx { get; }
}
