using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project")]
public class AzDmsProjectTask
{
    public AzDmsProjectTask(
        AzDmsProjectTaskCancel cancel,
        AzDmsProjectTaskCreate create,
        ICommand internalCommand
    )
    {
        CancelCommands = cancel;
        CreateCommands = create;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDmsProjectTaskCancel CancelCommands { get; }

    public AzDmsProjectTaskCreate CreateCommands { get; }

    public async Task<CommandResult> Cancel(AzDmsProjectTaskCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckName(AzDmsProjectTaskCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDmsProjectTaskCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Cutover(AzDmsProjectTaskCutoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDmsProjectTaskDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDmsProjectTaskListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzDmsProjectTaskRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDmsProjectTaskShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzDmsProjectTaskStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}