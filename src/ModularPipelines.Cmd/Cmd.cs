using ModularPipelines.Cmd.Models;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Cmd;

internal class Cmd : ICmd
{
    private readonly IPipelineContext _context;

    public Cmd(IPipelineContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }
}