using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi")]
public class AzSqlMiStartStopSchedule
{
    public AzSqlMiStartStopSchedule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSqlMiStartStopScheduleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlMiStartStopScheduleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStartStopScheduleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlMiStartStopScheduleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStartStopScheduleListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlMiStartStopScheduleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStartStopScheduleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlMiStartStopScheduleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStartStopScheduleUpdateOptions(), token);
    }
}