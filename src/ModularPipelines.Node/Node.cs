using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Node;

internal class Node : INode
{
    private readonly IPipelineContext _context;

    public INpm Npm { get; }

    public INvm Nvm { get; }

    public INpx Npx { get; }

    public Node(INpm npm, INvm nvm, IPipelineContext context, INpx npx)
    {
        _context = context;
        Npx = npx;
        Npm = npm;
        Nvm = nvm;
    }

    public virtual Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync("node", ["-v"], cancellationToken);
    }
}
