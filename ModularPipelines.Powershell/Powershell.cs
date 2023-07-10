using ModularPipelines.Models;
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

    public Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }
}
