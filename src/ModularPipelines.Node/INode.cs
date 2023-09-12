using ModularPipelines.Models;

namespace ModularPipelines.Node;

public interface INode
{
    Task<CommandResult> Version(CancellationToken cancellationToken = default);
    public INpm Npm { get; }
    public INvm Nvm { get; }
    public INpx Npx { get; }
}