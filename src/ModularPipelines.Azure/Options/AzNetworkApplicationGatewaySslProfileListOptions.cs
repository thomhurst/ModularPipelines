using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "ssl-profile", "list")]
public record AzNetworkApplicationGatewaySslProfileListOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;