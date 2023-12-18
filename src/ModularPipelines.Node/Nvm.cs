using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
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
            Arguments = new[] { "use", version },
        }, cancellationToken);
    }

    public Task<CommandResult> Install(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = new[] { "install", version },
        }, cancellationToken);
    }

    public Task<CommandResult> Version(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = new[] { "version" },
        }, cancellationToken);
    }

    public Task<CommandResult> Which(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = new[] { "which" },
        }, cancellationToken);
    }
}