using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "routeserver")]
public class AzNetworkRouteserverPeering
{
    public AzNetworkRouteserverPeering(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkRouteserverPeeringCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteserverPeeringDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkRouteserverPeeringListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAdvertisedRoutes(AzNetworkRouteserverPeeringListAdvertisedRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringListAdvertisedRoutesOptions(), token);
    }

    public async Task<CommandResult> ListLearnedRoutes(AzNetworkRouteserverPeeringListLearnedRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringListLearnedRoutesOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteserverPeeringShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteserverPeeringUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteserverPeeringWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverPeeringWaitOptions(), token);
    }
}