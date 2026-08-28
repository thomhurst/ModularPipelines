using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;

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
        return _context.Shell.RunAsync("nvm", ["use", version], cancellationToken);
    }

    public virtual Task<CommandResult> InstallAsync(string version, CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync("nvm", ["install", version], cancellationToken);
    }

    public virtual Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync("nvm", ["version"], cancellationToken);
    }

    public virtual Task<CommandResult> WhichAsync(CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync("nvm", ["which"], cancellationToken);
    }
}
