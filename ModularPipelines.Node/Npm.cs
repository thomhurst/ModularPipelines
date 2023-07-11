using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

internal class Npm : INpm
{
    private readonly IModuleContext _context;

    public Npm(IModuleContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Install(NpmInstallOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }
}
