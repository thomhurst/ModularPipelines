using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

internal class Nvm : INvm
{
    private readonly IPipelineContext _context;

    public Nvm(IPipelineContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Use(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = new[] { "use", version }
        }, cancellationToken);
    }
}
