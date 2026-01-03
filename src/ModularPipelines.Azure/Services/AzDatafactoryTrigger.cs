using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDatafactoryTriggerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetEventSubscriptionStatus(AzDatafactoryTriggerGetEventSubscriptionStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerGetEventSubscriptionStatusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDatafactoryTriggerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> QueryByFactory(AzDatafactoryTriggerQueryByFactoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerQueryByFactoryOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDatafactoryTriggerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzDatafactoryTriggerStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzDatafactoryTriggerStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SubscribeToEvent(AzDatafactoryTriggerSubscribeToEventOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerSubscribeToEventOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UnsubscribeFromEvent(AzDatafactoryTriggerUnsubscribeFromEventOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerUnsubscribeFromEventOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDatafactoryTriggerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDatafactoryTriggerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerWaitOptions(), cancellationToken: token);
    }
}