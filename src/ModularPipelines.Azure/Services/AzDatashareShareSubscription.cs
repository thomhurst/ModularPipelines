using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare")]
public class AzDatashareShareSubscription
{
    public AzDatashareShareSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelSynchronization(AzDatashareShareSubscriptionCancelSynchronizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDatashareShareSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatashareShareSubscriptionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareShareSubscriptionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDatashareShareSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSourceShareSynchronizationSetting(AzDatashareShareSubscriptionListSourceShareSynchronizationSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSynchronization(AzDatashareShareSubscriptionListSynchronizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSynchronizationDetail(AzDatashareShareSubscriptionListSynchronizationDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatashareShareSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareShareSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Synchronize(AzDatashareShareSubscriptionSynchronizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareShareSubscriptionSynchronizeOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatashareShareSubscriptionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareShareSubscriptionWaitOptions(), token);
    }
}