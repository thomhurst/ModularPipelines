using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-connection", "ipsec-policy", "list")]
public record AzNetworkVpnConnectionIpsecPolicyListOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;