using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Node.Services;

namespace ModularPipelines.Node;

internal class Node(INpm npm, INvm nvm, IPipelineContext context, INpx npx) : INode
{
    private readonly IPipelineContext _context = context;

    public INpm Npm { get; } = npm;

    public INvm Nvm { get; } = nvm;

    public INpx Npx { get; } = npx;

    public virtual Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync("node", ["-v"], cancellationToken);
    }
}
