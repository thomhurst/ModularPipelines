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

    public virtual Task<CommandResult> UseAsync(string version, CancellationToken cancellationToken = default)
    {
        return _context.Shell.Command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["use", version],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> InstallAsync(string version, CancellationToken cancellationToken = default)
    {
        return _context.Shell.Command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["install", version],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.Command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["version"],
        }, null, cancellationToken);
    }

    public virtual Task<CommandResult> WhichAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.Command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("nvm")
        {
            Arguments = ["which"],
        }, null, cancellationToken);
    }
}