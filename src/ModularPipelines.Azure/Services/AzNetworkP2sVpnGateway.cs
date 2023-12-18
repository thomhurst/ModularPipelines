using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkP2sVpnGateway
{
    public AzNetworkP2sVpnGateway(
        AzNetworkP2sVpnGatewayConnection connection,
        AzNetworkP2sVpnGatewayVpnClient vpnClient,
        ICommand internalCommand
    )
    {
        Connection = connection;
        VpnClient = vpnClient;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkP2sVpnGatewayConnection Connection { get; }

    public AzNetworkP2sVpnGatewayVpnClient VpnClient { get; }

    public async Task<CommandResult> Create(AzNetworkP2sVpnGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkP2sVpnGatewayDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkP2sVpnGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkP2sVpnGatewayListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkP2sVpnGatewayShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkP2sVpnGatewayUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkP2sVpnGatewayWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}