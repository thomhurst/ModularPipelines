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

    public virtual Task<CommandResult> ScriptAsync(CmdScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync(options, null, cancellationToken);
    }
}