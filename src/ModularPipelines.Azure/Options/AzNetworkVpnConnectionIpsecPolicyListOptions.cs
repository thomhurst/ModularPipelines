using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-connection", "ipsec-policy", "list")]
public record AzNetworkVpnConnectionIpsecPolicyListOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;