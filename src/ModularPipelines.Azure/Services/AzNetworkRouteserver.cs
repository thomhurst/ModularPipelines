using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkRouteserver
{
    public AzNetworkRouteserver(
        AzNetworkRouteserverPeering peering,
        ICommand internalCommand
    )
    {
        Peering = peering;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkRouteserverPeering Peering { get; }

    public async Task<CommandResult> Create(AzNetworkRouteserverCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteserverDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkRouteserverListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteserverShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteserverUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteserverWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteserverWaitOptions(), token);
    }
}