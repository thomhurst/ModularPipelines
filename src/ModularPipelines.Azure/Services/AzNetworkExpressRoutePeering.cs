using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route")]
public class AzNetworkExpressRoutePeering
{
    public AzNetworkExpressRoutePeering(
        AzNetworkExpressRoutePeeringConnection connection,
        AzNetworkExpressRoutePeeringPeerConnection peerConnection,
        ICommand internalCommand
    )
    {
        Connection = connection;
        PeerConnection = peerConnection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkExpressRoutePeeringConnection Connection { get; }

    public AzNetworkExpressRoutePeeringPeerConnection PeerConnection { get; }

    public async Task<CommandResult> Create(AzNetworkExpressRoutePeeringCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRoutePeeringDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStats(AzNetworkExpressRoutePeeringGetStatsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRoutePeeringListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRoutePeeringShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkExpressRoutePeeringUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRoutePeeringWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringWaitOptions(), token);
    }
}