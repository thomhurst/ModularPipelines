using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway")]
public class AzNetworkVnetGatewayVpnClient
{
    public AzNetworkVnetGatewayVpnClient(
        AzNetworkVnetGatewayVpnClientIpsecPolicy ipsecPolicy,
        ICommand internalCommand
    )
    {
        IpsecPolicy = ipsecPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVnetGatewayVpnClientIpsecPolicy IpsecPolicy { get; }

    public async Task<CommandResult> Generate(AzNetworkVnetGatewayVpnClientGenerateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayVpnClientGenerateOptions(), token);
    }

    public async Task<CommandResult> ShowHealth(AzNetworkVnetGatewayVpnClientShowHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayVpnClientShowHealthOptions(), token);
    }

    public async Task<CommandResult> ShowUrl(AzNetworkVnetGatewayVpnClientShowUrlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayVpnClientShowUrlOptions(), token);
    }
}