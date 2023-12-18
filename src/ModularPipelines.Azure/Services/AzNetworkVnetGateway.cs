using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVnetGateway
{
    public AzNetworkVnetGateway(
        AzNetworkVnetGatewayAad aad,
        AzNetworkVnetGatewayIpsecPolicy ipsecPolicy,
        AzNetworkVnetGatewayNatRule natRule,
        AzNetworkVnetGatewayPacketCapture packetCapture,
        AzNetworkVnetGatewayRevokedCert revokedCert,
        AzNetworkVnetGatewayRootCert rootCert,
        AzNetworkVnetGatewayVpnClient vpnClient,
        ICommand internalCommand
    )
    {
        Aad = aad;
        IpsecPolicy = ipsecPolicy;
        NatRule = natRule;
        PacketCapture = packetCapture;
        RevokedCert = revokedCert;
        RootCert = rootCert;
        VpnClient = vpnClient;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVnetGatewayAad Aad { get; }

    public AzNetworkVnetGatewayIpsecPolicy IpsecPolicy { get; }

    public AzNetworkVnetGatewayNatRule NatRule { get; }

    public AzNetworkVnetGatewayPacketCapture PacketCapture { get; }

    public AzNetworkVnetGatewayRevokedCert RevokedCert { get; }

    public AzNetworkVnetGatewayRootCert RootCert { get; }

    public AzNetworkVnetGatewayVpnClient VpnClient { get; }

    public async Task<CommandResult> Create(AzNetworkVnetGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVnetGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayDeleteOptions(), token);
    }

    public async Task<CommandResult> DisconnectVpnConnections(AzNetworkVnetGatewayDisconnectVpnConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayDisconnectVpnConnectionsOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVnetGatewayListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAdvertisedRoutes(AzNetworkVnetGatewayListAdvertisedRoutesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBgpPeerStatus(AzNetworkVnetGatewayListBgpPeerStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayListBgpPeerStatusOptions(), token);
    }

    public async Task<CommandResult> ListLearnedRoutes(AzNetworkVnetGatewayListLearnedRoutesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayListLearnedRoutesOptions(), token);
    }

    public async Task<CommandResult> Reset(AzNetworkVnetGatewayResetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayResetOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVnetGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayShowOptions(), token);
    }

    public async Task<CommandResult> ShowSupportedDevices(AzNetworkVnetGatewayShowSupportedDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayShowSupportedDevicesOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVnetGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVnetGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayWaitOptions(), token);
    }
}