using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "activity-log")]
public class AzMonitorActivityLogAlert
{
    public AzMonitorActivityLogAlert(
        AzMonitorActivityLogAlertActionGroup actionGroup,
        AzMonitorActivityLogAlertScope scope,
        ICommand internalCommand
    )
    {
        ActionGroup = actionGroup;
        Scope = scope;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorActivityLogAlertActionGroup ActionGroup { get; }

    public AzMonitorActivityLogAlertScope Scope { get; }

    public async Task<CommandResult> Create(AzMonitorActivityLogAlertCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorActivityLogAlertDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogAlertDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorActivityLogAlertListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogAlertListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorActivityLogAlertShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogAlertShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorActivityLogAlertUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogAlertUpdateOptions(), token);
    }
}