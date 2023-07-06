using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public class Npm : INpm
{
    private readonly IModuleContext _context;

    public Npm(IModuleContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Install(NpmInstallOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options.WithArguments(new[]{ options.Target ?? string.Empty }), cancellationToken);
    }

    public Task<CommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string>();

        arguments.AddNonNullOrEmpty(options.Target);

        if (options.Arguments?.Any() == true)
        {
            arguments.Add("--");
            arguments.AddRange(options.Arguments);
        }

        return _context.Command.ExecuteCommandLineTool(options.WithArguments(arguments), cancellationToken);
    }
}
