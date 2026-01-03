using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
public class AzNetworkfabricInternetgateway
{
    public AzNetworkfabricInternetgateway(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzNetworkfabricInternetgatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricInternetgatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricInternetgatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricInternetgatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayWaitOptions(), cancellationToken: token);
    }
}