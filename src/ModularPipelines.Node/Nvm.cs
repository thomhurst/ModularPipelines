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

    public virtual Task<CommandResult> Use(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = ["use", version],
        }, cancellationToken);
    }

    public virtual Task<CommandResult> Install(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = ["install", version],
        }, cancellationToken);
    }

    public virtual Task<CommandResult> Version(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = ["version"],
        }, cancellationToken);
    }

    public virtual Task<CommandResult> Which(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
        {
            Arguments = ["which"],
        }, cancellationToken);
    }
}