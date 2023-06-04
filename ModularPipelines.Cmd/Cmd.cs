using CliWrap.Buffered;
using ModularPipelines.Cmd.Models;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;

namespace ModularPipelines.Cmd;

public class Cmd<T> : ICmd<T>
{
    private readonly IModuleContext<T> _context;

    public Cmd(IModuleContext<T> context)
    {
        _context = context;
    }
    
    public Task<BufferedCommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "/c" };

        if (!options.Echo)
        {
            arguments.Add("/q");
        }
        
        arguments.Add(options.Script);

        return _context.Command().UsingCommandLineTool(options.ToCommandLineToolOptions("cmd", arguments), cancellationToken);
    }
}