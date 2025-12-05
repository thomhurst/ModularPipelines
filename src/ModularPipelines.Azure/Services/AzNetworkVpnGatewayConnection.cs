using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-gateway")]
public class AzNetworkVpnGatewayConnection
{
    public AzNetworkVpnGatewayConnection(
        AzNetworkVpnGatewayConnectionIpsecPolicy ipsecPolicy,
        AzNetworkVpnGatewayConnectionVpnSiteLinkConn vpnSiteLinkConn,
        ICommand internalCommand
    )
    {
        IpsecPolicy = ipsecPolicy;
        VpnSiteLinkConn = vpnSiteLinkConn;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnGatewayConnectionIpsecPolicy IpsecPolicy { get; }

    public AzNetworkVpnGatewayConnectionVpnSiteLinkConn VpnSiteLinkConn { get; }

    public async Task<CommandResult> Create(AzNetworkVpnGatewayConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnGatewayConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVpnGatewayConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnGatewayConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVpnGatewayConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnGatewayConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayConnectionWaitOptions(), token);
    }
}