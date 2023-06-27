using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

public class Node : INode
{
    private readonly IModuleContext _context;
    public INpm Npm { get; }
    public INvm Nvm { get; }

    public Node(INpm npm, INvm nvm, IModuleContext context)
    {
        _context = context;
        Npm = npm;
        Nvm = nvm;
    }

    public Task<BufferedCommandResult> Version(CancellationToken cancellationToken = default)
    {
        return _context.Command.UsingCommandLineTool(new CommandLineToolOptions("node")
        {
            Arguments = new []{ "-v" }
        }, cancellationToken);
    }
}