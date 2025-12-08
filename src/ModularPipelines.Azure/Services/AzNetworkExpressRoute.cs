using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkExpressRoute
{
    public AzNetworkExpressRoute(
        AzNetworkExpressRouteAuth auth,
        AzNetworkExpressRouteGateway gateway,
        AzNetworkExpressRoutePeering peering,
        AzNetworkExpressRoutePort port,
        ICommand internalCommand
    )
    {
        Auth = auth;
        Gateway = gateway;
        Peering = peering;
        Port = port;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkExpressRouteAuth Auth { get; }

    public AzNetworkExpressRouteGateway Gateway { get; }

    public AzNetworkExpressRoutePeering Peering { get; }

    public AzNetworkExpressRoutePort Port { get; }

    public async Task<CommandResult> Create(AzNetworkExpressRouteCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRouteDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteDeleteOptions(), token);
    }

    public async Task<CommandResult> GetStats(AzNetworkExpressRouteGetStatsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRouteListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteListOptions(), token);
    }

    public async Task<CommandResult> ListArpTables(AzNetworkExpressRouteListArpTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteListArpTablesOptions(), token);
    }

    public async Task<CommandResult> ListRouteTables(AzNetworkExpressRouteListRouteTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteListRouteTablesOptions(), token);
    }

    public async Task<CommandResult> ListRouteTablesSummary(AzNetworkExpressRouteListRouteTablesSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteListRouteTablesSummaryOptions(), token);
    }

    public async Task<CommandResult> ListServiceProviders(AzNetworkExpressRouteListServiceProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteListServiceProvidersOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRouteShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkExpressRouteUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRouteWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRouteWaitOptions(), token);
    }
}