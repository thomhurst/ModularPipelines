using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection")]
public class AzNetworkVpnGatewayConnectionVpnSiteLinkConn
{
    public AzNetworkVpnGatewayConnectionVpnSiteLinkConn(
        AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicy ipsecPolicy,
        ICommand internalCommand
    )
    {
        IpsecPolicy = ipsecPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicy IpsecPolicy { get; }

    public async Task<CommandResult> Add(AzNetworkVpnGatewayConnectionVpnSiteLinkConnAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkVpnGatewayConnectionVpnSiteLinkConnListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkVpnGatewayConnectionVpnSiteLinkConnRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}