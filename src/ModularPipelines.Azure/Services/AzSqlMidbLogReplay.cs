using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "midb")]
public class AzSqlMidbLogReplay
{
    public AzSqlMidbLogReplay(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Complete(AzSqlMidbLogReplayCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbLogReplayCompleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSqlMidbLogReplayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbLogReplayShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzSqlMidbLogReplayStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzSqlMidbLogReplayStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbLogReplayStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSqlMidbLogReplayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbLogReplayWaitOptions(), cancellationToken: token);
    }
}