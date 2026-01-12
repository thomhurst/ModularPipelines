using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

internal class Node : INode
{
    private readonly IPipelineHookContext _context;

    public INpm Npm { get; }

    public INvm Nvm { get; }

    public INpx Npx { get; }

    public Node(INpm npm, INvm nvm, IPipelineHookContext context, INpx npx)
    {
        _context = context;
        Npx = npx;
        Npm = npm;
        Nvm = nvm;
    }

    public virtual Task<CommandResult> Version(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("node")
        {
            Arguments = ["-v"],
        }, null, cancellationToken);
    }
}