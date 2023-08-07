using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Bash : IBash
{
    private readonly ICommand _command;

    public Bash(ICommand command)
    {
        _command = command;
    }

    public Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }
}
