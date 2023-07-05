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
        var arguments = new List<string> { "install" };

        arguments.AddNonNullOrEmpty(options.Target);

        return _context.Command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }

    public Task<CommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "ci" };

        return _context.Command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }

    public Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "run" };

        arguments.AddNonNullOrEmpty(options.Target);

        if (options.Arguments?.Any() == true)
        {
            arguments.Add("--");
            arguments.AddRange(options.Arguments);
        }

        return _context.Command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }
}
