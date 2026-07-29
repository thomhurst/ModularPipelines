using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Powershell : IPowerShellContext
{
    private readonly ICommandContext _command;

    public Powershell(ICommandContext command)
    {
        _command = command;
    }

    public virtual Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }
}