using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "connection")]
public class AzNetworkManagerConnectionSubscription
{
    public AzNetworkManagerConnectionSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkManagerConnectionSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerConnectionSubscriptionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerConnectionSubscriptionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkManagerConnectionSubscriptionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerConnectionSubscriptionListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerConnectionSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerConnectionSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerConnectionSubscriptionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerConnectionSubscriptionUpdateOptions(), token);
    }
}