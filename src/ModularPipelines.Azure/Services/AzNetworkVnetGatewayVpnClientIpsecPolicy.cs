using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "vpn-client")]
public class AzNetworkVnetGatewayVpnClientIpsecPolicy
{
    public AzNetworkVnetGatewayVpnClientIpsecPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Set(AzNetworkVnetGatewayVpnClientIpsecPolicySetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVnetGatewayVpnClientIpsecPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayVpnClientIpsecPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVnetGatewayVpnClientIpsecPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayVpnClientIpsecPolicyWaitOptions(), token);
    }
}