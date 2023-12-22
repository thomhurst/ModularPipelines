using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams")]
public class AzAmsLiveEvent
{
    public AzAmsLiveEvent(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAmsLiveEventCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsLiveEventDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAmsLiveEventListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reset(AzAmsLiveEventResetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventResetOptions(), token);
    }

    public async Task<CommandResult> Show(AzAmsLiveEventShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventShowOptions(), token);
    }

    public async Task<CommandResult> Standby(AzAmsLiveEventStandbyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStandbyOptions(), token);
    }

    public async Task<CommandResult> Start(AzAmsLiveEventStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzAmsLiveEventStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmsLiveEventUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzAmsLiveEventWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventWaitOptions(), token);
    }
}