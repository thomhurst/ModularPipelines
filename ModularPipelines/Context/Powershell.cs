using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Powershell : IPowershell
{
    private readonly ICommand _command;

    public Powershell(ICommand command)
    {
        _command = command;
    }

    public Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }
}
