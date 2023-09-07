using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
internal class Npm : INpm
{
    private readonly IPipelineContext _context;

    public Npm(IPipelineContext context)
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
