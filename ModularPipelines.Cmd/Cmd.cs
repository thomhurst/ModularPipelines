using ModularPipelines.Models;
using ModularPipelines.Cmd.Models;
using ModularPipelines.Context;

namespace ModularPipelines.Cmd;

internal class Cmd : ICmd
{
    private readonly IModuleContext _context;

    public Cmd(IModuleContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }
}
