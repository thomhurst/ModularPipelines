using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public class Npm<T> : INpm<T>
{
    private readonly IModuleContext<T> _context;

    public Npm(IModuleContext<T> context)
    {
        _context = context;
    }
    
    public Task<BufferedCommandResult> Install(NpmInstallOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "install" };

        arguments.AddNonNullOrEmpty(options.Target);

        if (options.Global)
        {
            arguments.Add("-g");
        }
        
        if (options.Force)
        {
            arguments.Add("--force");
        }
        
        if (options.DryRun)
        {
            arguments.Add("--dry-run");
        }
        
        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }

    public Task<BufferedCommandResult> CleanInstall(NpmCleanInstallOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "ci" };

        arguments.AddNonNullOrEmptyArgumentWithPrefix("--install-strategy=", options.InstallStrategy);
        arguments.AddRangeNonNullOrEmptyArgumentWithPrefix("--omit=", options.Omit);

        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }

    public Task<BufferedCommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "run" };

        arguments.AddNonNullOrEmpty(options.Target);

        if (options.Arguments?.Any() == true)
        {
            arguments.Add("--");
            arguments.AddRange(options.Arguments);
        }

        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("npm", arguments), cancellationToken);
    }
}