using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-cert", "list")]
public record AzNetworkApplicationGatewaySslCertListOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;