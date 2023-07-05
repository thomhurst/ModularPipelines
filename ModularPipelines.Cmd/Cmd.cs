using ModularPipelines.Models;
using ModularPipelines.Cmd.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;

namespace ModularPipelines.Cmd;

public class Cmd : ICmd
{
    private readonly IModuleContext _context;

    public Cmd(IModuleContext context)
    {
        _context = context;
    }
    
    public Task<CommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "/c" };

        if (!options.Echo)
        {
            arguments.Add("/q");
        }
        
        arguments.Add(options.Script);

        return _context.Command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("cmd", arguments), cancellationToken);
    }
}