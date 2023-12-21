using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "gateway")]
public class AzNetworkExpressRouteGatewayConnection
{
    public AzNetworkExpressRouteGatewayConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkExpressRouteGatewayConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRouteGatewayConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRouteGatewayConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRouteGatewayConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkExpressRouteGatewayConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRouteGatewayConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayConnectionWaitOptions(), token);
    }
}