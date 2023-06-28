using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Powershell.Models;

namespace ModularPipelines.Powershell;

public class Powershell : IPowershell
{
    private readonly IModuleContext _context;

    public Powershell(IModuleContext context)
    {
        _context = context;
    }
    
    public Task<BufferedCommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "-Command", options.Script };
        
        arguments.AddRangeNonNullOrEmpty(options.Arguments);
        
        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("pwsh", arguments), cancellationToken);
    }

    public Task<BufferedCommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default)
    {
        var arguments = new List<string> { "-File", options.FilePath };
        
        arguments.AddRangeNonNullOrEmpty(options.Arguments);
        
        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("pwsh", arguments), cancellationToken);
    }
}