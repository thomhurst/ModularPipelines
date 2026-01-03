using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route")]
public class AzNetworkExpressRouteGateway
{
    public AzNetworkExpressRouteGateway(
        AzNetworkExpressRouteGatewayConnection connection,
        ICommand internalCommand
    )
    {
        Connection = connection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkExpressRouteGatewayConnection Connection { get; }

    public async Task<CommandResult> Create(AzNetworkExpressRouteGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRouteGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRouteGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRouteGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkExpressRouteGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRouteGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteGatewayWaitOptions(), cancellationToken: token);
    }
}