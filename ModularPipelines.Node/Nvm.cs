using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

public class Nvm<T> : INvm<T>
{
    private readonly IModuleContext<T> _context;

    public Nvm(IModuleContext<T> context)
    {
        _context = context;
    }
    
    public Task<BufferedCommandResult> Use(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.UsingCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = new []{ "use", version }
        }, cancellationToken);
    }
}