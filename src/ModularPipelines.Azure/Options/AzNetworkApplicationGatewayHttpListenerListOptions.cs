using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "http-listener", "list")]
public record AzNetworkApplicationGatewayHttpListenerListOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;