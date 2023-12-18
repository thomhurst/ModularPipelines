using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare")]
public class AzDatashareProviderShareSubscription
{
    public AzDatashareProviderShareSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Adjust(AzDatashareProviderShareSubscriptionAdjustOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatashareProviderShareSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reinstate(AzDatashareProviderShareSubscriptionReinstateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareProviderShareSubscriptionReinstateOptions(), token);
    }

    public async Task<CommandResult> Revoke(AzDatashareProviderShareSubscriptionRevokeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareProviderShareSubscriptionRevokeOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatashareProviderShareSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareProviderShareSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatashareProviderShareSubscriptionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareProviderShareSubscriptionWaitOptions(), token);
    }
}