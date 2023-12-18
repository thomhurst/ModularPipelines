using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
public class AzDatafactoryTrigger
{
    public AzDatafactoryTrigger(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatafactoryTriggerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatafactoryTriggerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerDeleteOptions(), token);
    }

    public async Task<CommandResult> GetEventSubscriptionStatus(AzDatafactoryTriggerGetEventSubscriptionStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerGetEventSubscriptionStatusOptions(), token);
    }

    public async Task<CommandResult> List(AzDatafactoryTriggerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> QueryByFactory(AzDatafactoryTriggerQueryByFactoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerQueryByFactoryOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatafactoryTriggerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzDatafactoryTriggerStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzDatafactoryTriggerStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerStopOptions(), token);
    }

    public async Task<CommandResult> SubscribeToEvent(AzDatafactoryTriggerSubscribeToEventOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerSubscribeToEventOptions(), token);
    }

    public async Task<CommandResult> UnsubscribeFromEvent(AzDatafactoryTriggerUnsubscribeFromEventOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerUnsubscribeFromEventOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryTriggerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatafactoryTriggerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerWaitOptions(), token);
    }
}