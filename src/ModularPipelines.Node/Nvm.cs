using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
internal class Nvm : INvm
{
    private readonly IPipelineHookContext _context;

    public Nvm(IPipelineHookContext context)
    {
        _context = context;
    }

    public virtual Task<CommandResult> Use(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["use", version],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> Install(string version, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["install", version],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> Version(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["version"],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> Which(CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["which"],
        }, null, cancellationToken);
    }
}