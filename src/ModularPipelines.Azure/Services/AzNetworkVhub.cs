using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVhub
{
    public AzNetworkVhub(
        AzNetworkVhubBgpconnection bgpconnection,
        AzNetworkVhubConnection connection,
        AzNetworkVhubRoute route,
        AzNetworkVhubRouteMap routeMap,
        AzNetworkVhubRouteTable routeTable,
        AzNetworkVhubRoutingIntent routingIntent,
        ICommand internalCommand
    )
    {
        Bgpconnection = bgpconnection;
        Connection = connection;
        Route = route;
        RouteMap = routeMap;
        RouteTable = routeTable;
        RoutingIntent = routingIntent;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVhubBgpconnection Bgpconnection { get; }

    public AzNetworkVhubConnection Connection { get; }

    public AzNetworkVhubRoute Route { get; }

    public AzNetworkVhubRouteMap RouteMap { get; }

    public AzNetworkVhubRouteTable RouteTable { get; }

    public AzNetworkVhubRoutingIntent RoutingIntent { get; }

    public async Task<CommandResult> Create(AzNetworkVhubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVhubDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubDeleteOptions(), token);
    }

    public async Task<CommandResult> GetEffectiveRoutes(AzNetworkVhubGetEffectiveRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubGetEffectiveRoutesOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVhubListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVhubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVhubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVhubWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubWaitOptions(), token);
    }
}