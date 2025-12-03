using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "private-link", "ip-config", "list")]
public record AzNetworkApplicationGatewayPrivateLinkIpConfigListOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--private-link")] string PrivateLink,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;