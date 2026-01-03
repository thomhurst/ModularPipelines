using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkVhub
{
    public AzNetworkVhub(
        AzNetworkVhubBgpconnection bgpconnection,
        AzNetworkVhubConnection connection,
        AzNetworkVhubRouteMap routeMap,
        AzNetworkVhubRouteTable routeTable,
        AzNetworkVhubRoutingIntent routingIntent,
        ICommand internalCommand
    )
    {
        Bgpconnection = bgpconnection;
        Connection = connection;
        RouteMap = routeMap;
        RouteTable = routeTable;
        RoutingIntent = routingIntent;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVhubBgpconnection Bgpconnection { get; }

    public AzNetworkVhubConnection Connection { get; }

    public AzNetworkVhubRouteMap RouteMap { get; }

    public AzNetworkVhubRouteTable RouteTable { get; }

    public AzNetworkVhubRoutingIntent RoutingIntent { get; }

    public async Task<CommandResult> Create(AzNetworkVhubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkVhubDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetEffectiveRoutes(AzNetworkVhubGetEffectiveRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubGetEffectiveRoutesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkVhubListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkVhubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkVhubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkVhubWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubWaitOptions(), cancellationToken: token);
    }
}