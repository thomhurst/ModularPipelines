using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Powershell : IPowershell, IPowerShellContext
{
    private readonly ICommand _command;

    public Powershell(ICommand command)
    {
        _command = command;
    }

    public virtual Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }
}