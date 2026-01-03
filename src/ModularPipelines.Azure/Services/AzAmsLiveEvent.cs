using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAmsLiveEventDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAmsLiveEventListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Reset(AzAmsLiveEventResetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventResetOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAmsLiveEventShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Standby(AzAmsLiveEventStandbyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStandbyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzAmsLiveEventStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzAmsLiveEventStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAmsLiveEventUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzAmsLiveEventWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsLiveEventWaitOptions(), cancellationToken: token);
    }
}